using PurgaLib.Events.EventSystem.Interfaces;
using VoiceChat;
using VoiceChat.Networking;

namespace PurgaLib.Events.EventArgs.Player;

public class PlayerVoiceChattingEventArgs : IEventArgs
{
    public global::PurgaLib.API.Features.Player Player { get; }

    public VoiceMessage VoiceMessage { get; set; }

    public VoiceChatChannel Channel => VoiceMessage.Channel;

    public bool IsAllowed { get; set; } = true;

    public PlayerVoiceChattingEventArgs(global::PurgaLib.API.Features.Player player, VoiceMessage voiceMessage)
    {
        Player = player;
        VoiceMessage = voiceMessage;
    }
}