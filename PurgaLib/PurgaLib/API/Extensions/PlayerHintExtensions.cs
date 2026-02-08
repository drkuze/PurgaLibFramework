using PurgaLib.API.Enums;
using PurgaLib.API.Features;
using PurgaLib.API.Features.HintSystem;

namespace PurgaLib.API.Extensions
{
    public static class PlayerHintExtensions
    {
        public static void ShowCustomHint(this Player player,
            string text,
            float duration = 3f,
            string id = null,
            int priority = 0,
            bool sticky = false,
            HintZone zone = HintZone.Middle)
        {
            var controller = HintService.Get(player);
            controller.Show(new HintElement(text, duration, id, priority, sticky, zone));
        }

        public static void RemoveCustomHint(this Player player, string id)
        {
            HintService.Get(player).Remove(id);
        }

        public static void ClearCustomHints(this Player player)
        {
            HintService.Get(player).Clear();
        }

        public static bool HasCustomHint(this Player player, string id)
        {
            return HintService.Get(player).GetHint(id) != null;
        }
    }
}
