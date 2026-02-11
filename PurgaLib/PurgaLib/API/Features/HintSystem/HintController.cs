using System.Collections.Generic;
using System.Linq;
using System.Text;
using PurgaLib.API.Enums;

namespace PurgaLib.API.Features.HintSystem
{
    public class HintController
    {
        private readonly Player _player;

        private readonly List<HintElement> _top = new();
        private readonly List<HintElement> _middle = new();
        private readonly List<HintElement> _bottom = new();

        private string _lastRendered;

        public HintController(Player player)
        {
            _player = player;
        }

        public void Show(HintElement hint)
        {
            var list = GetZoneList(hint.Zone);

            if (!string.IsNullOrEmpty(hint.Id))
                list.RemoveAll(h => h.Id == hint.Id);

            list.Add(hint);
        }

        public void Show(string text, float duration, HintZone zone = HintZone.Middle)
        {
            Show(new HintElement(text, duration, zone: zone));
        }

        public void Remove(string id)
        {
            _top.RemoveAll(x => x.Id == id);
            _middle.RemoveAll(x => x.Id == id);
            _bottom.RemoveAll(x => x.Id == id);
        }

        public void Clear()
        {
            _top.Clear();
            _middle.Clear();
            _bottom.Clear();
            _lastRendered = null;
        }

        public HintElement GetHint(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _top.Concat(_middle).Concat(_bottom).FirstOrDefault(h => h.Id == id);
        }

        public bool HasHint(string id) => GetHint(id) != null;

        public void Tick()
        {
            RemoveExpired(_top);
            RemoveExpired(_middle);
            RemoveExpired(_bottom);

            string text = BuildFinalText();

            if (text == _lastRendered)
                return;

            _lastRendered = text;

            _player.ShowHint(text, 1.1f);
        }

        private void RemoveExpired(List<HintElement> list)
            => list.RemoveAll(h => h.Expired);

        private List<HintElement> GetZoneList(HintZone zone) => zone switch
        {
            HintZone.Top => _top,
            HintZone.Bottom => _bottom,
            _ => _middle
        };

        private string BuildFinalText()
        {
            StringBuilder sb = new();

            AppendZone(sb, _top);
            AppendZone(sb, _middle);
            AppendZone(sb, _bottom);

            return sb.ToString();
        }

        private void AppendZone(StringBuilder sb, List<HintElement> list)
        {
            if (list.Count == 0)
                return;

            foreach (var hint in list.OrderByDescending(h => h.Priority))
                sb.AppendLine(hint.GetRenderedText());

            sb.AppendLine();
        }
    }
}
