using System;
using System.Collections.Generic;
using LabApi.Features.Wrappers; 

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibCustomItems
{
    public static class CustomItemsDatabase
    {
        private static readonly List<CustomItem> Items = new();

        public static IReadOnlyList<CustomItem> AllItems => Items;
        
        public static void RegisterItem(CustomItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (Items.Contains(item)) return;

            Items.Add(item);
            item.SubscribeEvent();
        }

        public static void UnregisterItem(CustomItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (!Items.Contains(item)) return;

            item.UnSubscribeEvents();
            Items.Remove(item);
        }
        
        public static CustomItem GetItemById(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return Items.Find(x => x.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
        }
        
        public static bool IsCustom(string id)
        {
            return GetItemById(id) != null;
        }
    }
    
    public abstract class CustomItem
    {
        public abstract string Id { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract ItemType Type { get; }
        
        public abstract void SubscribeEvent();
        public abstract void UnSubscribeEvents();
        
        public abstract void OnUse(Player player);
    }
}