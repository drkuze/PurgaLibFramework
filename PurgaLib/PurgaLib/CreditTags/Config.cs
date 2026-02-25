using PurgaLib.API.Interfaces;

namespace PurgaLib.CreditTags
{
    public class Config : IConfig
    {
        public bool Enabled { get; set; } = true;
    }
}