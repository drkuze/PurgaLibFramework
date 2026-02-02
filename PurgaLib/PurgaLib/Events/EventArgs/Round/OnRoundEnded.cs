using PurgaLib.Events.EventSystem.Interfaces;

namespace PurgaLib.Events.EventArgs.Round;

public class RoundEndedEventArgs : IEventArgs
{
    public RoundSummary.LeadingTeam LeadingTeam { get; }
    public RoundEndedEventArgs(RoundSummary.LeadingTeam team) => LeadingTeam = team;
    public bool IsAllowed { get; set; }
}
