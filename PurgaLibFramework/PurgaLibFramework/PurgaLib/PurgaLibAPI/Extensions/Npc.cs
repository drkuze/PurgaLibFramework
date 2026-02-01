using System.Collections.Generic;
using MEC;
using Mirror;
using PlayerRoles;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features;
using UnityEngine;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Extensions
{
    public class Npc
    {
        private static int _nextConId = 1000;
        private static readonly List<Npc> Npcs = new();
        public static IReadOnlyList<Npc> List => Npcs;

        private CoroutineHandle _followHandle;

        public Player Player { get; }

        private Npc(Player player)
        {
            Player = player;
        }
        
        public static Npc Spawn(string name, RoleTypeId role, Vector3 position)
        {
            GameObject obj = Object.Instantiate(NetworkManager.singleton.playerPrefab);
            var fakeConn = new NetworkConnectionToClient(_nextConId++);
            NetworkServer.AddPlayerForConnection(fakeConn, obj);

            var hub = obj.GetComponent<ReferenceHub>();
            
            int id = _nextConId++;
            hub.nicknameSync.Network_myNickSync = name;
            hub.roleManager.InitializeNewRole(RoleTypeId.None, RoleChangeReason.None);
            hub.authManager.UserId = $"{id}";

            var playerWrapper = Player.Get(hub);
            playerWrapper.Role.Set(role);
            playerWrapper.Position = position;

            var npc = new Npc(playerWrapper);
            Npcs.Add(npc);
            return npc;
        }
        
        public string Name => Player.Nickname;
        public RoleTypeId Role => Player.Role.Type;
        public Vector3 Position { get => Player.Position; set => Player.Position = value; }
        public bool IsAlive => Player.IsAlive;
        
        public void SetRole(RoleTypeId role) => Player.Role.Set(role);

        public void Kill(string reason = "NPC killed")
        {
            if (!Player.IsAlive) return;
            Player.Kill(reason);
        }

        public void Destroy(string reason = "NPC destroyed")
        {
            NetworkServer.Destroy(Player.ReferenceHub.gameObject);
            Npcs.Remove(this);
        }

        public void TeleportToPlayer(Player target)
        {
            if (target == null || !IsAlive) return;
            Position = target.Position + Vector3.back;
        }

        public void FollowPlayer(Player target, float speed = 5f, float updateRate = 0.05f, float smoothness = 0.1f)
        {
            if (_followHandle.IsRunning)
                Timing.KillCoroutines(_followHandle);

            _followHandle = Timing.RunCoroutine(FollowRoutine(target, speed, updateRate, smoothness));
        }

        private IEnumerator<float> FollowRoutine(Player target, float speed, float updateRate, float smoothness)
        {
            while (target != null && IsAlive)
            {
                Vector3 targetPos = target.Position + Vector3.back;
                Position = Vector3.Lerp(Position, targetPos, speed * smoothness * updateRate);
                yield return Timing.WaitForSeconds(updateRate);
            }
        }

        public void StopFollow()
        {
            if (_followHandle.IsRunning)
                Timing.KillCoroutines(_followHandle);
        }
    }
}
