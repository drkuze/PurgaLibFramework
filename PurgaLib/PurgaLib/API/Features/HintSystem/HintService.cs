using System.Collections.Generic;

namespace PurgaLib.API.Features.HintSystem
{
    public static class HintService
    {
        private static readonly Dictionary<Player, HintController> Controllers = new();

        public static HintController Get(Player player)
        {
            if (player == null) return null;

            if (!Controllers.TryGetValue(player, out var controller))
            {
                controller = new HintController(player);
                Controllers[player] = controller;
            }

            return controller;
        }
        
        public static void TickAll()
        {
            foreach (var controller in Controllers.Values)
                controller.Tick();
        }

        public static void Remove(Player player)
        {
            if (player == null) return;

            if (Controllers.TryGetValue(player, out var controller))
                controller.Clear();

            Controllers.Remove(player);
        }

        public static void ClearAll()
        {
            foreach (var c in Controllers.Values)
                c.Clear();

            Controllers.Clear();
        }
    }
}
