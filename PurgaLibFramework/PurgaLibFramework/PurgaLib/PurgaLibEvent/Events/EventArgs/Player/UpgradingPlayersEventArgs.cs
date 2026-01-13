using Scp914;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player;

public class UpgradingPlayersEventArgs : System.EventArgs
{
    public PurgaLibAPI.Features.Player Player { get; }
    public Scp914KnobSetting Setting { get;}

    public UpgradingPlayersEventArgs(PurgaLibAPI.Features.Player player, Scp914KnobSetting setting)
    {
        Player = player;
        Setting = setting;
    }
    
}