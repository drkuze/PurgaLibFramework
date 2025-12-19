namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Round;

public class RoundEndedEventArgs : System.EventArgs
{
    public string LeadingTeam { get; }
    public RoundEndedEventArgs(string team) => LeadingTeam = team;
}
