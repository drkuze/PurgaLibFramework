using PlayerRoles;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventSystem.Interfaces;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player
{
    public class RespawnedTeamEventArgs : IEventArgs
    {
        public Team Team { get; }
        
        public int Count { get; }
        
        public bool IsAllowed { get; set; } = true;
        
        public RespawnedTeamEventArgs(Team team, int count)
        {
            Team = team;
            Count = count;
        }
    }
}
