using HarmonyLib;
using LabApi.Loader.Features.Plugins;
using LabApi.Loader.Features.Plugins.Enums;
using PurgaLib.API.Core;
using PurgaLib.API.Features.Server;
using PurgaLib.CustomItems.Handlers;
using PurgaLib.Loader.PurgaLib_Loader.LoaderEvent;
using System;
using System.IO;
using System.Reflection;

namespace PurgaLib.Loader
{
    public class Loader : Plugin<Config>
    {
        private PurgaLoader _purgaLoader;
        private PEventRegister _register;
        public static Loader Instance { get; private set; }
        public override string Name { get; } = "Loader";
        public override string Description { get; } = "The loader of PurgaLib";
        public override string Author { get; } = "PurgaLib Team";
        public override Version Version { get; } = new Version(2, 4, 0);
        public override Version RequiredApiVersion { get; } = new Version(2, 4, 0);
        public override LoadPriority Priority { get; } = LoadPriority.Lowest;
        public static string ApiVersion => "1.5.0"; //<-- Added Coin, Pickup, PrefabHelper, Respawning, Scp914, TeslaGate, WorkStation, Added more features to Server and Player.
        public static readonly string _PurgaFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        public override void Enable()
        {
            Instance = this;
            
            Harmony.DEBUG = true;     
            var harmony = new Harmony("PurgaLib");
            var assembly = Assembly.GetExecutingAssembly();
            harmony.PatchAll(assembly);

            _register = new PEventRegister();
            _register.RegisterAll();

            CustomItemHandler.RegisterNaturalEvent();

            PurgaUpdater.Initialize();
            PurgaUpdater.Instance.CheckUpdate();

            Logged.SendRaw($"API Version {ApiVersion}", ConsoleColor.Magenta);
            Logged.SendRaw($"PurgaLib Version: {Version}", ConsoleColor.Magenta);
            Logged.SendRaw(@" 
Welcome to:

██████╗ ██╗   ██╗██████╗  ██████╗  █████╗ ██╗     ██╗██████╗ 
██╔══██╗██║   ██║██╔══██╗██╔════╝ ██╔══██╗██║     ██║██╔══██╗
██████╔╝██║   ██║██████╔╝██║  ███╗███████║██║     ██║██████╔╝
██╔═══╝ ██║   ██║██╔══██╗██║   ██║██╔══██║██║     ██║██╔══██╗
██║     ╚██████╔╝██║  ██║╚██████╔╝██║  ██║███████╗██║██████╔╝
╚═╝      ╚═════╝ ╚═╝  ╚═╝ ╚═════╝ ╚═╝  ╚═╝╚══════╝╚═╝╚═════╝ [*] by PurgaLib Team
", ConsoleColor.Magenta);
            
            _purgaLoader = new PurgaLoader();
            _purgaLoader.LoadAllPlugins();
        }
        

        public override void Disable()
        {
            Instance = null;
            _purgaLoader?.UnloadPlugins();
            Logged.SendRaw("Bye bye from PurgaLib", ConsoleColor.Magenta);
            _purgaLoader?.UnloadPlugins();
            _register.UnRegisterAll();
        }
    }
    
}
