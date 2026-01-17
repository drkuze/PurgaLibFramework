using System;
using LabApi.Loader.Features.Plugins;
using LabApi.Loader.Features.Plugins.Enums;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Server;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibCustomItems.Handlers;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibCustomRoles.Handlers;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Attribute;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibLoader.PurgaLib_Loader.LoaderEvent;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibLoader
{
    public class Loader : Plugin<Config>
    {
        private PurgaLoader _purgaLoader;
        public static Loader Instance { get; private set; }
        public override string Name { get; } = "PurgaLibLoader";
        public override string Description { get; } = "The loader of PurgaLib";
        public override string Author { get; } = "Florentina";
        public override Version Version { get; } = new Version(1, 8, 0);
        public override Version RequiredApiVersion { get; } = new Version(1, 0, 0, 0);
        public override LoadPriority Priority { get; } = LoadPriority.Lowest;

        public override void Enable()
        {
            Instance = this;
            PurgaUpdater.Initialize();
            PurgaUpdater.Instance.CheckUpdate();
            
            PatchALL.PatchAll();
            Log.Success("Event's Loaded!");
            Log.SendRaw($"PurgaLibAPI Version {PurgaLibAPI.Version.version}", ConsoleColor.DarkRed);
            Log.SendRaw($"PurgaLib Version: {Version}", ConsoleColor.Red);
            Log.SendRaw(@" 
Welcome to:

██████╗ ██╗   ██╗██████╗  ██████╗  █████╗ ██╗     ██╗██████╗ 
██╔══██╗██║   ██║██╔══██╗██╔════╝ ██╔══██╗██║     ██║██╔══██╗
██████╔╝██║   ██║██████╔╝██║  ███╗███████║██║     ██║██████╔╝
██╔═══╝ ██║   ██║██╔══██╗██║   ██║██╔══██║██║     ██║██╔══██╗
██║     ╚██████╔╝██║  ██║╚██████╔╝██║  ██║███████╗██║██████╔╝
╚═╝      ╚═════╝ ╚═╝  ╚═╝ ╚═════╝ ╚═╝  ╚═╝╚══════╝╚═╝╚═════╝ [*] by Florentina
", ConsoleColor.DarkMagenta);
            
            _purgaLoader = new PurgaLoader();
            _purgaLoader.LoadPlugins();
        }
        

        public override void Disable()
        {
            Instance = null;
            Log.SendRaw("Bye bye from PurgaLib", ConsoleColor.Cyan);
            _purgaLoader?.UnloadPlugins();
        }
    }
}
