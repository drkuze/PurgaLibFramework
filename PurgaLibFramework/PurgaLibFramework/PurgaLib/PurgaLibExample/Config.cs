using System.ComponentModel;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.PluginManager;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibExample;

public class Config : IPurgaConfig
{
    public bool Enabled { get; set; } = true;

    [Description("Example string config")] 
    public string Message => "Welcome on the server.";
}