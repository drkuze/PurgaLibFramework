using System;

namespace PurgaLib.API.Features
{
    public readonly struct Badge
    {
        public Badge(string text, string color, bool isGlobal = false)
        {
            Text = text ?? string.Empty;
            Color = color ?? "#FFFFFF";
            IsGlobal = isGlobal;
        }
        
        public string Text { get; }
        
        public string Color { get; }
        
        public bool IsGlobal { get; }
        
        public static bool IsValidColor(string hex)
        {
            if (string.IsNullOrWhiteSpace(hex))
                return false;

            foreach (var pair in Misc.AllowedColors)
            {
                if (pair.Value.Equals(hex, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }
        
        public static bool TryGetColorType(string hex, out Misc.PlayerInfoColorTypes type)
        {
            foreach (var pair in Misc.AllowedColors)
            {
                if (pair.Value.Equals(hex, StringComparison.OrdinalIgnoreCase))
                {
                    type = pair.Key;
                    return true;
                }
            }

            type = default;
            return false;
        }
        
        public static string GetHex(Misc.PlayerInfoColorTypes type)
            => Misc.AllowedColors[type];

        public override string ToString()
            => $"{Text} ({Color}) [{(IsGlobal ? "Global" : "Local")}]";
    }
}
