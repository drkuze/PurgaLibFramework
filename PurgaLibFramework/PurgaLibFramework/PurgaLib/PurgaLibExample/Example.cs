using System;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Server;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.PluginManager;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibExample;

public class Example : Plugin<Config>
{
    public static Example Instance { get; private set; }
    
    public override string Name { get; } = "Example plugin";
    public override string Author { get; } = "Ofc Florentina";
    public override string Description { get; } = "Example plugin for PurgaLibFramework";
    public override Version Version { get; } = new Version(1,0,0,0);
    public override Version RequiredPurgaLibVersion { get; } = new Version(0,0,7);
    protected override void OnEnabled()
    {
        Instance = this;
        Log.Info("Hi from Example Plugin!");
    }

    protected override void OnDisabled()
    {
        Instance = null;
        Log.Info("Bye from Example Plugin!");
    }
}