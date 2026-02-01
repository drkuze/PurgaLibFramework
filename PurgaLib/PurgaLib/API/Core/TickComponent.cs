namespace PurgaLib.API.Core
{
    public abstract class TickComponent : PComponent
    {
        public bool Enabled { get; set; } = true;

        protected abstract void Tick();
    }
}
