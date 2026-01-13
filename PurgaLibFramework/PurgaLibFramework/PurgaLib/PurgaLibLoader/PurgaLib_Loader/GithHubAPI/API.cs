using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibLoader.PurgaLib_Loader.GithHubAPI;

internal static class GitHubApi
{
    public static GitHubRelease[] GetReleases(HttpClient client, long repoId)
    {
        string url = $"https://api.github.com/repositories/{repoId}/releases";
        client.DefaultRequestHeaders.UserAgent.ParseAdd("PurgaLib-Updater");
        string json = client.GetStringAsync(url).Result;
        return JsonSerializer.Deserialize<GitHubRelease[]>(json) ?? Array.Empty<GitHubRelease>();
    }
}

internal sealed class GitHubRelease
{
    [JsonPropertyName("tag_name")]
    public string Tag { get; set; }

    [JsonPropertyName("assets")]
    public GitHubAsset[] Assets { get; set; }
}

internal sealed class GitHubAsset
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("browser_download_url")]
    public string Url { get; set; }
}