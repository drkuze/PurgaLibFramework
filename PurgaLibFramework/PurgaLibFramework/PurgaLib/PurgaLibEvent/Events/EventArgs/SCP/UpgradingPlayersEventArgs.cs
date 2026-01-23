using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventSystem.Interfaces;
using Scp914;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.SCP;

public class UpgradingPlayersEventArgs : IEventArgs
{
    public global::PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player Player { get; }
    public Scp914KnobSetting Setting { get;}

    public UpgradingPlayersEventArgs(global::PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player player, Scp914KnobSetting setting)
    {
        Player = player;
        Setting = setting;
    }

    public bool IsAllowed { get; set; }
}