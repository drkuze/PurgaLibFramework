using PurgaLib.Events.EventSystem.Interfaces;
using Scp914;

namespace PurgaLib.Events.EventArgs.Map;

public class UpgradingPlayersEventArgs : IEventArgs
{
    public global::PurgaLib.API.Features.Player Player { get; }
    public Scp914KnobSetting Setting { get;}

    public UpgradingPlayersEventArgs(global::PurgaLib.API.Features.Player player, Scp914KnobSetting setting)
    {
        Player = player;
        Setting = setting;
    }

    public bool IsAllowed { get; set; }
}