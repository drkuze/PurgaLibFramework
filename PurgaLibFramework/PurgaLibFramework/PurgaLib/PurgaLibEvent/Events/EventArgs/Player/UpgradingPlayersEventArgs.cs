using Scp914;

namespace PurgaLibEvents.PurgaLibEvent.Events.EventArgs.Player;

public class UpgradingPlayersEventArgs : System.EventArgs
{
    public PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player Player { get; }
    public Scp914KnobSetting Setting { get;}

    public UpgradingPlayersEventArgs(PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player player, Scp914KnobSetting setting)
    {
        Player = player;
        Setting = setting;
    }
    
}