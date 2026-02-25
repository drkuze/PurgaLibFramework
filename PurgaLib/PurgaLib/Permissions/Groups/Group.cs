using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace PurgaLib.Permissions.Groups;

public static class GroupsHandler 
{
    public static Dictionary<string, Group> GroupDict { get; set; } = new();
}

public class Group
{
    [YamlMember(Alias = "default")]
    public bool IsDefault { get; set; }

    public List<string> Permissions { get; set; } = new();
}