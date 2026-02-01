using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Pickups;
using InventorySystem.Items.Usables;
using UnityEngine;
using System;
using InventorySystem.Items.Keycards;

namespace PurgaLib.API.Features
{
    public class Item
    {
        public ItemBase Base { get; set; }
        public Player Owner { get; set; }
        public ushort Serial => Base.ItemSerial;
        public ItemType Type => Base.ItemTypeId;

        public bool IsEquipped => Owner != null && Owner.Inventory.CurItem.SerialNumber == Serial;
        public bool IsPickup => Pickup != null;

#pragma warning disable
        public ItemPickupBase Pickup { get; private set; }
#pragma warning restore

        public Vector3 Position =>
            Pickup != null ? Pickup.transform.position :
            Owner != null ? Owner.Transform.position :
            Vector3.zero;

        #region Constructors

        public Item(ItemBase itemBase, Player owner = null)
        {
            Base = itemBase ?? throw new ArgumentNullException(nameof(itemBase));
            Owner = owner;
        }

        public Item(ItemType type, Player owner = null)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));

            Base = owner.Inventory.ServerAddItem(type, ItemAddReason.Undefined);
            Owner = owner;
        }

        #endregion

        #region Type Helpers

        public bool IsWeapon => Base is InventorySystem.Items.Firearms.Firearm;
        public Firearm Firearm => Base is InventorySystem.Items.Firearms.Firearm f ? new(f, this) : null;
        public bool IsConsumable => Base is Consumable;
        public Consumable Consumable => Base as Consumable;

        public bool IsKeycard => Base is KeycardItem;
        public KeycardItem Keycard => Base as KeycardItem;

        #endregion

        #region Actions

        public void Destroy()
        {
            if (Pickup != null)
            {
                UnityEngine.Object.Destroy(Pickup.gameObject);
                return;
            }

            Owner?.Inventory.ServerRemoveItem(Serial, null);
        }

        public void Drop(Vector3 position)
        {
            if (Owner == null) return;
            Owner.Inventory.ServerDropItem(Serial);
        }

        public void Give(Player target)
        {
            if (Owner == null || target == null) return;

            Owner.Inventory.ServerRemoveItem(Serial, null);
            target.Inventory.ServerAddItem(Type, ItemAddReason.Undefined, Serial);

            ChangeOwner(Owner, target);
        }

        public void ChangeOwner(Player oldOwner, Player newOwner)
        {
            Base.OnRemoved(null);
            Owner = newOwner;
            Base.Owner = newOwner.ReferenceHub;
            Base.OnAdded(null);
        }

        #endregion

        #region Factory

        public static Item Create(ItemType type, Player owner = null) => new Item(type, owner);

        public static Item Get(ItemBase baseItem, Player owner = null)
        {
            return new Item(baseItem, owner);
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return $"Item(Type={Type}, Serial={Serial}, Owner={Owner?.Nickname ?? "None"})";
        }

        #endregion
    }
}