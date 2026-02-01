using PurgaLib.API.Extensions.AttachmentExtension;
using PurgaLib.API.Features;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Firearm : Item
{
    public InventorySystem.Items.Firearms.Firearm BaseFirearm { get; }
    public Item ParentItem { get; }

    public List<AttachmentIdentifier> Attachments { get; private set; } = new();

    public Firearm(InventorySystem.Items.Firearms.Firearm firearm, Item parent)
        : base(firearm, parent?.Owner)
    {
        BaseFirearm = firearm;
        ParentItem = parent;
    }

    public void AddAttachment(AttachmentIdentifier attachment) => AddAttachment(new List<AttachmentIdentifier> { attachment });

    public void AddAttachment(IEnumerable<AttachmentIdentifier> attachments)
    {
        foreach (var att in attachments)
        {
            if (!Attachments.Any(a => a.Id == att.Id))
                Attachments.Add(att);
        }
        ApplyAttachments();
    }

    private void ApplyAttachments()
    {
        Debug.Log($"Firearm {BaseFirearm.ItemTypeId} now has {Attachments.Count} attachments: {string.Join(", ", Attachments.Select(a => a.Name))}");
    }
}