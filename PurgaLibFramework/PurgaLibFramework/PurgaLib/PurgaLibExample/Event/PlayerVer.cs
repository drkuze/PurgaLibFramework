using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Server;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibExample.Event;

public class PlayerVer
{
    private void PlayerVeri(PlayerVerifiedEventArgs ev)
    {
        Log.Success($"Player successfully entered: {ev.Player.Nickname}");
        ev.Player.SendBroadcast(Example.Instance.Config.Message, 6);
    }

    public void Register()
    {
        PurgaLibEvent.Events.Handler.PlayerHandler.Verified += PlayerVeri;
    }

    public void UnRegister()
    {
        PurgaLibEvent.Events.Handler.PlayerHandler.Verified -= PlayerVeri;
    }
}