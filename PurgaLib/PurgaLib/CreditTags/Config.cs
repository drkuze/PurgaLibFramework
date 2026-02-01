using PurgaLib.API.Features.PluginManager;

namespace PurgaLib.CreditTags
{
    public class Config : IConfig
    {
        public bool Enabled { get; set; } = true;
    }
}