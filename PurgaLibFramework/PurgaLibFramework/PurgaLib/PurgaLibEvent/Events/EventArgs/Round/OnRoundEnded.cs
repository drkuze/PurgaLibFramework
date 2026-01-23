using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventSystem.Interfaces;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Round;

public class RoundEndedEventArgs : IEventArgs
{
    public RoundSummary.LeadingTeam LeadingTeam { get; }
    public RoundEndedEventArgs(RoundSummary.LeadingTeam team) => LeadingTeam = team;
    public bool IsAllowed { get; set; }
}
