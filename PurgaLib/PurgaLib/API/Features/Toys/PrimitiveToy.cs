using AdminToys;

namespace PurgaLib.API.Features.Toys
{
    public class PrimitiveToy : AdminToy
    {
        public PrimitiveObjectToy Primitive => (PrimitiveObjectToy)Base;

        internal PrimitiveToy(PrimitiveObjectToy toy) : base(toy) { }
    }
}
