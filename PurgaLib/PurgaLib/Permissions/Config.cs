using System.ComponentModel;
using System.IO;
using PurgaLib.API.Core;
using PurgaLib.API.Features.PluginManager;

namespace PurgaLib.Permissions;

public class Config : IConfig
{
    public Config()
    {
        Folder = PurgaPaths.Configs;
        FullPath = Path.Combine(Folder, "permissions.yml");
    }
    public bool Enabled { get; set; } = true;
    [Description("Where should be the folder.")]
    public string Folder { get; private set; }
    public string FullPath { get; private set; }
}