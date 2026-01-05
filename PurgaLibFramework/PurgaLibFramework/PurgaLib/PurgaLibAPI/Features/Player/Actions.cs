using PlayerRoles;
using UnityEngine;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player
{
    public class Player
    {
        internal LabApi.Features.Wrappers.Player Base { get; }

        internal Player(LabApi.Features.Wrappers.Player player)
        {
            Base = player;
        }

        public string UserId => Base.UserId;
        public string Nickname => Base.Nickname;

        public void Kill(string reason)
        {
            Base.Kill(reason);
        }

        public void Heal(int amount)
        {
            Base.Heal(amount);
        }

        public void Teleport(Vector3 position)
        {
            Base.Position = position;
        }

        public void Teleport(float x, float y, float z)
        {
            Base.Position.Set(x, y, z);
        }

        public void TeleportRelative(float dx, float dy, float dz)
        {
            var pos = Base.Position;
            Base.Position.Set(pos.x + dx, pos.y + dy, pos.z + dz);
        }

        public void GiveItem(ItemType item)
        {
            Base.AddItem(item);
        }

        public void SetRole(RoleTypeId role)
        {
            Base.SetRole(role);
        }

        public static Player Get(LabApi.Features.Wrappers.Player player)
            => player == null ? null : new Player(player);
    }
}