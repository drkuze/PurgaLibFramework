using HarmonyLib;
using LabApi.Loader.Features.Plugins;
using LabApi.Loader.Features.Plugins.Enums;
using PurgaLib.API.Core;
using PurgaLib.API.Features.Server;
using PurgaLib.CustomItems.Handlers;
using PurgaLib.Loader.PurgaLib_Loader.LoaderEvent;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using LabApi.Features;

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
        public override Version Version { get; } = new Version(2, 9, 0);
        public override Version RequiredApiVersion { get; } = new Version(LabApiProperties.CompiledVersion);
        public override LoadPriority Priority { get; } = LoadPriority.Lowest;
        public static string ApiVersion => "1.9.0"; //<-- Added Admin toys, Better CreditTags, Added coordinates for hint system.
        public static readonly string _PurgaFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        static Loader()
        {
            AppDomain.CurrentDomain.AssemblyResolve += ResolveEmbeddedAssembly;
        }
        
        private static Assembly ResolveEmbeddedAssembly(object sender, ResolveEventArgs args)
        {
            var asmName = new AssemblyName(args.Name).Name + ".dll";

            var currentAsm = Assembly.GetExecutingAssembly();
            var resourceName = currentAsm.GetManifestResourceNames()
                .FirstOrDefault(r => r.EndsWith(asmName));

            if (resourceName == null)
                return null;

            using (var stream = currentAsm.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    return null;

                byte[] data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);
                return Assembly.Load(data);
            }
        }
        
        public override void Enable()
        {
            Instance = this;
            
            Harmony.DEBUG = true;     
            var harmony = new Harmony("PurgaLib");
            var assembly = Assembly.GetExecutingAssembly();
            harmony.PatchAll(assembly);

            _register = new PEventRegister();

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
        }
    }
    
}
