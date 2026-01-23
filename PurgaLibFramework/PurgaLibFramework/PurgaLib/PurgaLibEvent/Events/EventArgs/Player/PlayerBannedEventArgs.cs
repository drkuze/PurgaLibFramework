using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventSystem.Interfaces;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player;

public class PlayerBannedEventArgs : IEventArgs
{
    public PurgaLibAPI.Features.Player Player { get; }
    public BanHandler.BanType BanType { get; }

    public PlayerBannedEventArgs(PurgaLibAPI.Features.Player player, BanHandler.BanType banType)
    {
        Player = player;
        BanType = banType;
    }

    public bool IsAllowed { get; set; }
}