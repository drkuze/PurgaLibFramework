using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CommandSystem;
using PurgaLib.API.Features.PluginManager;
using PurgaLib.API.Features.Server;
using PurgaLib.Loader.PurgaLib_Loader;
using PurgaLib.Permissions.Groups;
using Query;
using RemoteAdmin;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Player = PurgaLib.API.Features.Player;

namespace PurgaLib.Permissions;

public class Permissions : Plugin<Config>
{
    public static Permissions Singleton { get; private set; }
    public override string Name { get; } = "PurgaLib.Permissions";
    public override string Author { get; } = "PurgaLib Team";
    public override string Description { get; } = "Permissions System for the framework";
    public override Version Version { get; } = new Version(1, 0, 0);
    public override Version RequiredPurgaLibVersion { get; } = new Version(PurgaLibProperties.CurrVersion);
    public static string DefaultYaml = Encoding.UTF8.GetString(Resources.Resources.permissions);
    
    private static readonly ISerializer Serializer = new SerializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .IgnoreFields()
            .Build();

        private static readonly IDeserializer Deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .IgnoreFields()
            .IgnoreUnmatchedProperties()
            .Build();
        
    protected override void OnEnabled()
    {
        Singleton = this;
        MEC.Timing.CallDelayed(3f, () =>
        {
            Create();
        });
    }

    protected override void OnDisabled()
    {
        GroupsHandler.GroupDict.Clear();
        Singleton = null;
    }
    
    public static void Save() 
    {
        if (Singleton?.Config?.FullPath == null) return;
        
        string toSave = Serializer.Serialize(GroupsHandler.GroupDict);
        
        File.WriteAllText(Singleton.Config.FullPath, toSave);
    }

    public static void Create()
    {
        try
        {
            if (!Directory.Exists(Singleton.Config.Folder))
            {
                Directory.CreateDirectory(Singleton.Config.Folder);
            }
            
            if (!File.Exists(Singleton.Config.FullPath))
            {
                File.WriteAllText(Singleton.Config.FullPath, DefaultYaml);
            }
            string rawYaml = File.ReadAllText(Singleton.Config.FullPath);
            GroupsHandler.GroupDict = Deserializer.Deserialize<Dictionary<string, Group>>(rawYaml);
        }
        catch (Exception e)
        {
            Logged.Error($"[PurgaLib] Error during permissions init: {e}");
        }
    }
    
    public static bool CheckPermission(ICommandSender sender, string permission) => CheckPermission(sender as CommandSender, permission);
    
    public static bool CheckPermission(CommandSender sender, string permission)
    {
        if (sender == null) return false;
        if (sender.FullPermissions || sender is ServerConsoleSender)
        {
            return true;
        }
        
        if (sender is PlayerCommandSender || sender is QueryCommandSender)
        {
            var player = Player.Get(sender.SenderId);
            
            if (player == null) return false;
            
            return CheckPermission(player, permission); 
        }

        return false;
    }
    

    public static bool CheckPermission(Player player, string permission)
    {
        if (string.IsNullOrEmpty(permission)) return false;
        
        if (player.ReferenceHub.isLocalPlayer) return true;
        
        string groupKey = player.ReferenceHub.serverRoles.Group?.BadgeText;

        Group group = null;
        
        if (!string.IsNullOrEmpty(groupKey))
        {
            GroupsHandler.GroupDict.TryGetValue(groupKey, out group);
        }
        
        if (group == null)
        {
            group = GroupsHandler.GroupDict.Values.FirstOrDefault(g => g.IsDefault);
        }

        if (group == null) return false;
        
        if (group.Permissions.Contains("*") || group.Permissions.Contains(".*"))
            return true;
        
        if (group.Permissions.Any(p => p.Equals(permission, StringComparison.OrdinalIgnoreCase)))
            return true;
        
        string[] parts = permission.Split('.');
        string currentPath = "";
        for (int i = 0; i < parts.Length - 1; i++)
        {
            currentPath = i == 0 ? parts[i] : $"{currentPath}.{parts[i]}";
            if (group.Permissions.Any(p => p.Equals($"{currentPath}.*", StringComparison.OrdinalIgnoreCase)))
                return true;
        }

        return false;
    }
}
