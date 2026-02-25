using CommandSystem;

namespace PurgaLib.Permissions.Extensions;

public static class PermissionsExtensions
{
    public static bool CheckPermission(this ICommandSender sender, string permission) 
        => Permissions.CheckPermission(sender, permission);

    public static bool CheckPermission(this CommandSender sender, string permission) 
        => Permissions.CheckPermission(sender, permission);

    public static bool CheckPermission(this API.Features.Player player, string permission) 
        => Permissions.CheckPermission(player, permission);
}