using System;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.PluginManager
{
    public interface IPurgaConfig
    {
        bool Enabled { get; set; }
    }
    
    public abstract class Plugin<TConfig> where TConfig : class, IPurgaConfig, new()
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