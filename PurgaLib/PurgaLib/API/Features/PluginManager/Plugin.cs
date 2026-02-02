using System;

namespace PurgaLib.API.Features.PluginManager
{
    public interface IConfig
    {
        bool Enabled { get; set; }
    }
    
    public abstract class Plugin<TConfig> where TConfig : class, IConfig, new()
    {
        public abstract string Name { get; }
        public abstract string Author { get; }
        public abstract string Description { get; }
        public abstract Version Version { get; }
        public abstract Version RequiredPurgaLibVersion { get; }
        public TConfig Config { get; set; } = new TConfig();
        
        
        protected abstract void OnEnabled();
        protected abstract void OnDisabled();
        
        public void Enable()
        {
            OnEnabled();
        }

        public void Disable()
        {
            OnDisabled();
        }
    }
}