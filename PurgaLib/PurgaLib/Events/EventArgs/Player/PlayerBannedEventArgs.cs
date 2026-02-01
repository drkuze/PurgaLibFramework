using PurgaLib.Events.EventSystem.Interfaces;

namespace PurgaLib.Events.EventArgs.Player;

public class PlayerBannedEventArgs : IEventArgs
{
    public global::PurgaLib.API.Features.Player Player { get; }
    public BanHandler.BanType BanType { get; }

    public PlayerBannedEventArgs(global::PurgaLib.API.Features.Player player, BanHandler.BanType banType)
    {
        Player = player;
        BanType = banType;
    }

    public bool IsAllowed { get; set; }
}