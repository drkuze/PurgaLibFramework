using System;
using System.Collections.Generic;
using InventorySystem.Items.Coin;

namespace PurgaLib.API.Features
{
    public class CoinItem : Item
    {
        public static new Dictionary<Coin, CoinItem> Dictionary { get; } = new();
        public static new IReadOnlyCollection<CoinItem> List => Dictionary.Values;

        public static CoinItem? Get(Coin? coin, Player owner = null)
        {
            if (coin == null) return null;

            if (Dictionary.TryGetValue(coin, out CoinItem item))
                return item;

            return new CoinItem(coin, owner);
        }

        internal CoinItem(Coin coin, Player owner = null) : base(coin, owner)
        {
            Base = coin;

            if (CanCache)
                Dictionary[coin] = this;
        }

        public new Coin Base { get; }

        public bool? LastFlipResult =>
            Coin.FlipTimes.TryGetValue(Serial, out double time) ? (time > 0.0) : null;

        public double? LastFlipTime =>
            Coin.FlipTimes.TryGetValue(Serial, out double time) ? Math.Abs(time) : null;

        public void RemoveFromCache()
        {
            Dictionary.Remove(Base);
        }
    }
}
