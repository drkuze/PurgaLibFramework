using PlayerStatsSystem;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Enums;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.DamageHandler
{
    public class DamageHandler : CustomReasonDamageHandler
    {
        public DamageHandler(
            Player victim,
            Player attacker,
            float amount,
            DamageType damageType,
            string cassieAnnouncement = null
        )
            : base(
                BuildReason(attacker, damageType),
                amount,
                cassieAnnouncement
            )
        {
            Attacker = attacker?.ReferenceHub;
        }

        public ReferenceHub Attacker { get; }

        private static string BuildReason(Player attacker, DamageType damageType)
        {
            string source = attacker != null
                ? attacker.Nickname
                : "Unknown";

            return $"{damageType} ({source})";
        }
    }
}
