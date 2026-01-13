using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using PluginAPI.Helpers;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibLoader.PurgaLib_Loader.GithHubAPI;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibLoader.PurgaLib_Loader.LoaderEvent
{
    internal sealed class PurgaUpdater
    {
        private const long RepoId = 0;
        private const string InstallerWin = "PurgaLib.Installer-Win.exe";
        private const string InstallerLinux = "PurgaLib.Installer-Linux";

        private static readonly PlatformID Platform = Environment.OSVersion.Platform;
        private static readonly Encoding Encoding = new UTF8Encoding(false, false);

        public static PurgaUpdater Instance { get; private set; }
        public bool Busy { get; private set; }

        private string Folder =>
            Directory.Exists(Path.Combine(Paths.Plugins, "global"))
                ? "global"
                : Server.Port.ToString();

        private string InstallerName =>
            Platform == PlatformID.Win32NT ? InstallerWin :
            Platform == PlatformID.Unix ? InstallerLinux : null;

        public static PurgaUpdater Initialize()
        {
            return Instance ??= new PurgaUpdater();
        }

        public void CheckUpdate()
        {
            if (Busy)
                return;

            try
            {
                using HttpClient client = CreateClient();
                if (FindUpdate(client, out GitHubRelease release, out GitHubAsset asset))
                    Update(client, release, asset);
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
            }
        }

        private HttpClient CreateClient()
        {
            HttpClient client = new()
            {
                Timeout = TimeSpan.FromMinutes(8)
            };

            client.DefaultRequestHeaders.Add(
                "User-Agent",
                $"PurgaLib ({Assembly.GetExecutingAssembly().GetName().Version})");

            return client;
        }

        private bool FindUpdate(HttpClient client, out GitHubRelease release, out GitHubAsset asset)
        {
            Thread.Sleep(5000);

            var current = Assembly.GetExecutingAssembly().GetName().Version;
            var releases = GitHubApi.GetReleases(client, RepoId);

            foreach (var r in releases)
            {
                if (Version.Parse(r.Tag) <= current)
                    continue;

                asset = r.Assets.FirstOrDefault(a =>
                    a.Name.Equals(InstallerName, StringComparison.OrdinalIgnoreCase));

                if (asset != null)
                {
                    release = r;
                    return true;
                }
            }

            release = null;
            asset = null;
            return false;
        }

        private void Update(HttpClient client, GitHubRelease release, GitHubAsset asset)
        {
            Busy = true;

            try
            {
                string serverPath = Environment.CurrentDirectory;
                string installerPath = Path.Combine(serverPath, asset.Name);

                using var response = client.GetAsync(asset.Url).Result;
                using var stream = response.Content.ReadAsStreamAsync().Result;
                using var fs = new FileStream(installerPath, FileMode.Create);

                stream.CopyTo(fs);

                if (Platform == PlatformID.Unix)
                    Process.Start("chmod", $"+x {installerPath}")?.WaitForExit();

                ProcessStartInfo info = new()
                {
                    FileName = installerPath,
                    WorkingDirectory = serverPath,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    Arguments =
                        $"--exit " +
                        $"{(Folder == "global" ? "" : $"--target-port {Folder}")} " +
                        $"--target-version {release.Tag} " +
                        $"--appdata \"{Paths.AppData}\""
                };

                var proc = Process.Start(info);
                if (proc == null)
                    return;

                proc.OutputDataReceived += (_, e) =>
                {
                    if (!string.IsNullOrWhiteSpace(e.Data))
                        Logger.Info($"[Updater] {e.Data}");
                };

                proc.ErrorDataReceived += (_, e) =>
                {
                    if (!string.IsNullOrWhiteSpace(e.Data))
                        Logger.Error($"[Updater] {e.Data}");
                };

                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();
                proc.WaitForExit();

                if (proc.ExitCode == 0)
                {
                    Logger.Warn("PurgaLib updated. Server will restart next round.");
                    ServerStatic.StopNextRound = ServerStatic.NextRoundAction.Restart;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
            }
            finally
            {
                Busy = false;
            }
        }
    }
}
