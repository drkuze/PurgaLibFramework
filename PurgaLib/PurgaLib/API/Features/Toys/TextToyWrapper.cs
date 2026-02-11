using AdminToys;

namespace PurgaLib.API.Features.Toys
{
    public class TextToyWrapper : AdminToy
    {
        public TextToy Text => (TextToy)Base;

        internal TextToyWrapper(TextToy toy) : base(toy) { }

        public string Content
        {
            get => Text.TextFormat;
            set => Text.Network_textFormat = value;
        }
    }
}
