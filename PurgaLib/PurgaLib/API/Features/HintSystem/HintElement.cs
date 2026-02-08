using UnityEngine;
using PurgaLib.API.Enums;

namespace PurgaLib.API.Features.HintSystem
{
    public class HintElement
    {
        public string Id { get; }
        public string Text { get; set; }
        public float Duration { get; }
        public float EndTime { get; }
        public int Priority { get; }
        public bool Sticky { get; }
        public HintZone Zone { get; }
        
        public bool UseProgressBar;
        public int BarSize = 10;
        public char FilledChar = '#';
        public char EmptyChar = '.';

        public HintElement(
            string text,
            float duration,
            string id = null,
            int priority = 0,
            bool sticky = false,
            HintZone zone = HintZone.Middle)
        {
            Text = text;
            Duration = duration;
            Id = id;
            Priority = priority;
            Sticky = sticky;
            Zone = zone;

            EndTime = Time.time + duration;
        }

        public bool Expired => !Sticky && Time.time >= EndTime;

        public string GetRenderedText()
        {
            if (!UseProgressBar) return Text;

            float progress = Mathf.Clamp01((EndTime - Time.time) / Duration);
            int filled = Mathf.RoundToInt(progress * BarSize);
            int empty = BarSize - filled;

            string bar = new string(FilledChar, filled) + new string(EmptyChar, empty);
            return $"{bar} {Text}";
        }
    }
}
