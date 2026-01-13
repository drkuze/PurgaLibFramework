namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Map
{
    public class DoorInteractingEventArgs : System.EventArgs
    {
        public PurgaLibAPI.Features.Player Player { get; }
        public PurgaLibAPI.Features.Door Door { get; }
        public bool IsAllowed { get; set; } = true;

        public DoorInteractingEventArgs(PurgaLibAPI.Features.Player player, PurgaLibAPI.Features.Door door)
        {
            Player = player;
            Door = door;
        }
    }
}