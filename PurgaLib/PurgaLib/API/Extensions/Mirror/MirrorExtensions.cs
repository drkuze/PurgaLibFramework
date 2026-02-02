using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using LabApi.Features.Wrappers;
using Mirror;
using PlayerRoles;
using UnityEngine;

namespace PurgaLib.API.Extensions.Mirror
{
    public static class MirrorExtensions
    {
        private static readonly Dictionary<Type, MethodInfo> Writers = new();
        private static readonly Dictionary<string, ulong> DirtyBits = new();
        private static readonly Dictionary<string, string> RpcNames = new();

        private static MethodInfo spawnMethod;

        private static void InitWriters()
        {
            if (Writers.Count != 0)
                return;

            foreach (var m in typeof(NetworkWriterExtensions).GetMethods().Where(x => x.GetParameters().Length == 2))
                Writers[m.GetParameters()[1].ParameterType] = m;
        }

        private static ulong GetDirtyBit(Type type, string property)
        {
            string key = $"{type.Name}.{property}";
            if (DirtyBits.TryGetValue(key, out ulong bit))
                return bit;

            var prop = type.GetProperty(property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var body = prop?.GetSetMethod()?.GetMethodBody();
            if (body == null)
                return 0;

            var bytes = body.GetILAsByteArray();
            int i = Array.LastIndexOf(bytes, (byte)OpCodes.Ldc_I8.Value);
            bit = bytes[i + 1];

            DirtyBits[key] = bit;
            return bit;
        }

        private static string GetRpc(Type type, string name)
        {
            string key = $"{type.Name}.{name}";
            if (RpcNames.TryGetValue(key, out string value))
                return value;

            var m = type.GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var body = m?.GetMethodBody();
            if (body == null)
                return null;

            var bytes = body.GetILAsByteArray();
            int i = Array.IndexOf(bytes, (byte)OpCodes.Ldstr.Value);
            value = m.Module.ResolveString(BitConverter.ToInt32(bytes, i + 1));

            RpcNames[key] = value;
            return value;
        }

        private static byte GetIndex(NetworkIdentity id, Type t)
        {
            return (byte)Array.FindIndex(id.NetworkBehaviours, x => x.GetType() == t);
        }

        public static void FakeSyncVar<T>(this Player player, NetworkIdentity id, Type behaviour, string property, T value)
        {
            if (!player.Connection.isReady)
                return;

            InitWriters();

            var writer = NetworkWriterPool.Get();
            writer.WriteULong(GetDirtyBit(behaviour, property));
            Writers[typeof(T)]?.Invoke(null, new object[] { writer, value });

            player.Connection.Send(new EntityStateMessage
            {
                netId = id.netId,
                payload = writer.ToArraySegment()
            });

            NetworkWriterPool.Return(writer);
        }

        public static void FakeRpc(this Player player, NetworkIdentity id, Type behaviour, string rpc, params object[] args)
        {
            if (!player.Connection.isReady)
                return;

            InitWriters();

            var writer = NetworkWriterPool.Get();

            foreach (var arg in args)
                Writers[arg.GetType()]?.Invoke(null, new[] { writer, arg });

            RpcMessage msg = new()
            {
                netId = id.netId,
                componentIndex = GetIndex(id, behaviour),
                functionHash = (ushort)GetRpc(behaviour, rpc).GetStableHashCode(),
                payload = writer.ToArraySegment()
            };

            player.Connection.Send(msg);
            NetworkWriterPool.Return(writer);
        }

        public static void RespawnFor(this Player player, NetworkIdentity id)
        {
            spawnMethod ??= typeof(NetworkServer).GetMethod("SendSpawnMessage", BindingFlags.NonPublic | BindingFlags.Static);

            player.Connection.Send(new ObjectDestroyMessage { netId = id.netId }, 0);
            spawnMethod.Invoke(null, new object[] { id, player.Connection });
        }

        public static void MoveFor(this Player player, NetworkIdentity id, Vector3 pos)
        {
            id.transform.position = pos;
            player.RespawnFor(id);
        }

        public static void ScaleFor(this Player player, NetworkIdentity id, Vector3 scale)
        {
            id.transform.localScale = scale;
            player.RespawnFor(id);
        }

        public static void MoveForAll(this NetworkIdentity id, Vector3 pos)
        {
            id.transform.position = pos;
            foreach (var p in Player.ReadyList)
                p.RespawnFor(id);
        }

        public static void ScaleForAll(this NetworkIdentity id, Vector3 scale)
        {
            id.transform.localScale = scale;
            foreach (var p in Player.ReadyList)
                p.RespawnFor(id);
        }
        public static void ChangeAppearance(this Player player, RoleTypeId type, bool skipJump = false)
        {
            if (!player.Connection.isReady)
                return;

            var writer = NetworkWriterPool.Get();

            writer.WriteUShort(38952);
            writer.WriteUInt(player.NetworkId);
            writer.WriteRoleType(type);

            foreach (var target in Player.ReadyList)
                target.Connection.Send(writer.ToArraySegment());

            NetworkWriterPool.Return(writer);

            if (!skipJump)
                player.Position += Vector3.up * 0.25f;
        }
    }
}
