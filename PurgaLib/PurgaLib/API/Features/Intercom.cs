using Mirror;
using PlayerRoles.Voice;
using UnityEngine;

namespace PurgaLib.API.Features
{
    public static class Intercom
    {
        public static IntercomDisplay Display => IntercomDisplay._singleton;

        public static string Text
        {
            get => Display.Network_overrideText;
            set => Display.Network_overrideText = value;
        }

        public static IntercomState State
        {
            get => PlayerRoles.Voice.Intercom.State;
            set => PlayerRoles.Voice.Intercom.State = value;
        }

        public static GameObject GameObject => PlayerRoles.Voice.Intercom._singleton.gameObject;

        public static Transform Transform => PlayerRoles.Voice.Intercom._singleton.transform;

        public static bool InUse => State is IntercomState.InUse or IntercomState.Starting;

        public static Player Speaker => !InUse ? null : Player.Get(PlayerRoles.Voice.Intercom._singleton._curSpeaker);

        public static double RemainingCooldown
        {
            get => PlayerRoles.Voice.Intercom._singleton.Network_nextTime - NetworkTime.time;
            set => PlayerRoles.Voice.Intercom._singleton.Network_nextTime = NetworkTime.time + value;
        }

        public static float SpeechRemainingTime
        {
            get => !InUse ? 0f : PlayerRoles.Voice.Intercom._singleton.RemainingTime;
            set => PlayerRoles.Voice.Intercom._singleton._nextTime = NetworkTime.time + value;
        }

        public static void PlaySound(bool starting) => PlayerRoles.Voice.Intercom._singleton.RpcPlayClip(starting);

        public static bool TrySetOverride(Player player, bool state) => PlayerRoles.Voice.Intercom.TrySetOverride(player?.ReferenceHub, state);

        public static bool HasOverride(Player player) => PlayerRoles.Voice.Intercom.HasOverride(player?.ReferenceHub);

        public static void Reset() => State = IntercomState.Ready;

        public static void Timeout() => State = IntercomState.Cooldown;
    }
}
