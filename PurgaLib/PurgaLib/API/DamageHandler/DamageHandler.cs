using PlayerStatsSystem;
using PurgaLib.API.Enums;
using PurgaLib.API.Features;

namespace PurgaLib.API.DamageHandler
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
