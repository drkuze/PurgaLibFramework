using AdminToys;
using UnityEngine;

namespace PurgaLib.API.Features.Toys
{
    public class LightToy : AdminToy
    {
        public LightSourceToy Light => (LightSourceToy)Base;

        internal LightToy(LightSourceToy toy) : base(toy) { }

        public Color Color
        {
            get => Light.LightColor;
            set => Light.NetworkLightColor = value;
        }
    }
}
