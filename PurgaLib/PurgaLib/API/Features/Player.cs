using CommandSystem;
using CustomPlayerEffects;
using Hints;
using InventorySystem;
using Mirror;
using PlayerRoles;
using PlayerStatsSystem;
using PurgaLib.API.Core;
using PurgaLib.API.Core.Interfaces;
using PurgaLib.API.Enums;
using PurgaLib.API.Extensions;
using PurgaLib.API.Extensions.AttachmentExtension;
using PurgaLib.API.Features.Effects;
using PurgaLib.API.Features.Players;
using PurgaLib.API.Features.Players.Hints;
using PurgaLib.API.Features.Role;
using RemoteAdmin;
using System.Collections.Generic;
using System.Linq;
using Footprinting;
using PlayerRoles.FirstPersonControl;
using UnityEngine;
using Utils.Networking;
using static ReferenceHub;

namespace PurgaLib.API.Features
{
    public class Player : PActor, IEntity, IWorldObject
    {
        private static readonly Dictionary<ReferenceHub, Player> Cache = new();

        public ReferenceHub ReferenceHub { get; }
        public PlayerEffectHandler EffectHandler { get; }
        public Player(ReferenceHub gameObject)
        {
            ReferenceHub = GetHub(gameObject);
            EffectHandler = new PlayerEffectHandler(gameObject);
        }
        public static Player Host
        {
            get
            {
                global::ReferenceHub.TryGetHostHub(out var hub);
                return hub == null ? null : Get(hub);
            }
        }
        public override Transform Transform => ReferenceHub.transform;

        public override bool IsAlive => ReferenceHub.IsAlive();

        public bool IsNtf => Team == Team.FoundationForces;
        public bool IsHuman => IsAlive && !IsScp;
        public bool IsScp => Team == Team.SCPs;
        public bool IsChaos => Team == Team.ChaosInsurgency;
        public bool IsTutorial => Role == RoleTypeId.Tutorial;


        public bool IsHost => ReferenceHub.isLocalPlayer;
        public bool IsNpc => !IsHost && ReferenceHub.connectionToClient.GetType() != typeof(NetworkConnectionToClient);
        public bool IsConnected => ReferenceHub.connectionToClient != null;
        public Footprint Footprint { get; private set; }

        public PlyHint CurrentHint { get; internal set; }
        public bool HasHint => CurrentHint != null;
        public HintDisplay HintDisplay { get; private set; }


#pragma warning disable CS0108
        public Vector3 Position
        {
            get => ReferenceHub.transform.position;
            set => ReferenceHub.transform.position = value;
        }

        public string UserId => ReferenceHub.authManager.UserId;
        public string Nickname => ReferenceHub.nicknameSync.DisplayName;
        public int PlayerId => ReferenceHub.PlayerId;
        public uint NetworkId => ReferenceHub.characterClassManager.netId;
        public NetworkConnection Connection => IsHost ? ReferenceHub.networkIdentity.connectionToServer : ReferenceHub.networkIdentity.connectionToClient;
        public PlayerRoleBase RoleBase => ReferenceHub.roleManager.CurrentRole;

        public float Health => ReferenceHub.playerStats.GetModule<HealthStat>().CurValue;
        public float MaxHealth => ReferenceHub.playerStats.GetModule<HealthStat>().MaxValue;
        public float MinHealth => ReferenceHub.playerStats.GetModule<HealthStat>().MinValue;

        public PlayerRole Role => new PlayerRole(this, ReferenceHub.roleManager.CurrentRole);
        public Badge? Badge
        {
            get
            {
                string text = ReferenceHub.serverRoles.Network_myText;
                string color = ReferenceHub.serverRoles.Network_myColor;

                if (string.IsNullOrEmpty(text))
                    return null;

                return new Badge(text, color, true);
            }

            set
            {
                if (value == null)
                {
                    ReferenceHub.serverRoles.SetText(null);
                    return;
                }

                ReferenceHub.serverRoles.SetText(value.Value.Text);
                ReferenceHub.serverRoles.SetColor(value.Value.Color);
            }
        }
        public Team Team => ReferenceHub.roleManager.CurrentRole.Team;
        public Inventory Inventory => ReferenceHub.inventory;
        
        public void Kill(string reason = null)
        {
            Hurt(null, 100000, DamageType.None, reason);
        }
        public void Hurt(Player attacker, float amount, DamageType damageType = DamageType.None, string cassieAnnouncement = null) =>
            Hurt(new DamageHandler.DamageHandler(this, attacker, amount, damageType, cassieAnnouncement));

        public void Hurt(DamageHandlerBase damageHandlerBase) => ReferenceHub.playerStats.DealDamage(damageHandlerBase);
        public void Heal(float amount, bool overrideMaxHealth = false)
        {
            var healthStat = ReferenceHub.playerStats.GetModule<HealthStat>();

            if (!overrideMaxHealth)
                healthStat.ServerHeal(amount);
            else
                healthStat.CurValue += amount;
        }


        public Item AddItem(ItemType type)
        {
            Item item = Item.Create(type, this);
            Inventory.UserInventory.Items[item.Serial] = item.Base;
            item.ChangeOwner(null, this);
            return item;
        }

        public Item AddItem(FirearmType firearmType, IEnumerable<AttachmentIdentifier> attachments = null)
        {
            Item item = AddItem(firearmType.GetItemType());

            if (item.IsWeapon && attachments != null)
            {
                item.Firearm.AddAttachment(attachments);
            }

            return item;
        }

        public IEnumerable<Item> AddItems(IEnumerable<ItemType> items)
        {
            List<Item> addedItems = new();
            foreach (var type in items)
                addedItems.Add(AddItem(type));
            return addedItems;
        }

        public IEnumerable<Item> AddItems(Dictionary<FirearmType, IEnumerable<AttachmentIdentifier>> firearms)
        {
            List<Item> added = new();
            foreach (var kvp in firearms)
                added.Add(AddItem(kvp.Key, kvp.Value));
            return added;
        }

        public void RemoveItem(Item item)
        {
            if (item == null) return;

            Inventory.ServerRemoveItem(item.Serial, null);
        }

        public void GiveItem(Item item, Player target)
        {
            if (item == null || target == null) return;

            RemoveItem(item);
            target.AddItem(item.Type);
        }

        public void DropItem(Item item, Vector3 position)
        {
            if (item == null) return;

            Inventory.ServerDropItem(item.Serial);
        }

        public void Teleport(Vector3 position) => ReferenceHub.transform.position = position;

        public void Broadcast(PlyBroadcast broadcast, bool clearPrevious = false)
        {
            if (broadcast == null || !broadcast.Show)
                return;

            if (clearPrevious)
                global::Broadcast.Singleton.TargetClearElements(Connection);

            global::Broadcast.Singleton.TargetAddElement(Connection, broadcast.Content, broadcast.Duration, broadcast.Type);
        }

        public void Broadcast(string message, ushort duration = 10, Broadcast.BroadcastFlags type = global::Broadcast.BroadcastFlags.Normal, bool clearPrevious = false)
        {
            Broadcast(new PlyBroadcast(message, duration, true, type), clearPrevious);
        }

        public void ShowHint(string message, float duration = 3f)
        {
            ShowHint(message,
                new HintParameter[] { new StringHintParameter(message) },
                null,
                duration);
        }
        public float CassieAnnouncement(Cassie.CassieAnnouncement cassieAnnouncement)
        {
            Cassie.CassieAnnouncementDispatcher.CurrentAnnouncement.OnStartedPlaying();
            var payload = cassieAnnouncement.Payload;
            if (!Cassie.CassieTtsAnnouncer.TryPlay(payload, out float total_duration))
                return 0;
            payload.SendToHubsConditionally(x => x == ReferenceHub);
            return total_duration;
        }
        
        public void ShowHint(string message, HintEffect[] hintEffects, float duration = 3f)
        {
            ShowHint(message,
                new HintParameter[] { new StringHintParameter(message) },
                hintEffects,
                duration);
        }

        public void ShowHint(string message, HintParameter[] hintParameters, HintEffect[] hintEffects, float duration = 3f)
        {
            if (ReferenceHub == null || ReferenceHub.hints == null)
                return;

            message ??= string.Empty;

            ReferenceHub.hints.Show(new TextHint(
                message,
                (hintParameters != null && hintParameters.Length > 0)
                    ? hintParameters
                    : new HintParameter[] { new StringHintParameter(message) },
                hintEffects,
                duration));
        }

        public void ShowHint(PlyHint hint)
        {
            if (hint == null)
                return;

            ShowHint(hint.Message, hint.Duration);
        }

        public void SendMessage(string message, string color) => ReferenceHub.gameConsoleTransmission.SendToClient(message, color);

        public void Kick(string reason) => ReferenceHub.connectionToClient?.Disconnect();

        public void Ban(BanDetails reason, BanHandler.BanType duration) => BanHandler.IssueBan(reason, duration);


        public void RankName(string text)
        {
            if (ReferenceHub == null) return;

            ReferenceHub.serverRoles.Network_myText = text;
        }

        public void RankColor(RankColorsType color)
        {
            if (ReferenceHub == null) return;

            ReferenceHub.serverRoles.Network_myColor = color.ToString();
        }

        public void SetScale(Vector3 scale)
        {
            ReferenceHub.transform.localScale = scale;
            new SyncedScaleMessages.ScaleMessage(scale, ReferenceHub).SendToAuthenticated();
        }
        public void SetScale(Vector3 scale, IEnumerable<Player> viewers)
        {
            ReferenceHub.transform.localScale = scale;
            new SyncedScaleMessages.ScaleMessage(scale, ReferenceHub).SendToHubsConditionally(x => x != null && viewers.Contains(Get(x)));
        }
        
        public bool EnableEffect(Effect effect) 
        {
            if (effect == null)
                return false;

            if (!effect.IsEnabled)
                return false;

            if (!EffectHandler.TryGetEffect(effect.Type, out StatusEffectBase status))
                return false;

            if (effect.AddDurationIfActive && status.IsEnabled)
            {
                status.ServerChangeDuration(effect.Duration, addDuration: true);
            }
            else
            {
                status.Intensity = effect.Intensity;
                status.ServerSetState(effect.Intensity, effect.Duration);
            }

            return true;
        }
        
        public bool DisableEffect(EffectType type)
        {
            if (!EffectHandler.TryGetEffect(type, out StatusEffectBase effect))
                return false;

            effect.ServerDisable();
            return true;
        }
        
        public bool HasEffect(EffectType type)
        {
            return EffectHandler.TryGetEffect(type, out StatusEffectBase effect)
                   && effect.IsEnabled;
        }

        public StatusEffectBase GetEffect(EffectType type)
        {
            EffectHandler.TryGetEffect(type, out StatusEffectBase effect);
            return effect;
        }

        public static IReadOnlyCollection<Player> List =>
            AllHubs
                .Select(Get)
                .Where(p => p != null)
                .ToList();

        public static int Count =>
            AllHubs.Count;


        public static Player Get(ReferenceHub hub)
        {
            if (hub == null)
                return null;

            if (!Cache.TryGetValue(hub, out var player))
            {
                player = new Player(hub);
                Cache.Add(hub, player);
            }

            return player;
        }


        public static Player Get(int playerId) =>
            Get(AllHubs.FirstOrDefault(h => h.PlayerId == playerId));

        public static Player Get(string userId) =>
            Get(AllHubs.FirstOrDefault(h => h.authManager.UserId == userId));

        public static Player Get(ICommandSender sender)
        {
            if (sender is PlayerCommandSender pcs)
                return Get(pcs.ReferenceHub);

            return null;
        }

        public static Player Get(GameObject gameObject)
        {
            if (gameObject == null)
                return null;

            ReferenceHub hub = gameObject.GetComponent<ReferenceHub>();

            if (hub == null)
                return null;

            return Get(hub);
        }

        public static bool TryGet(string userId, out Player player)
        {
            player = Get(userId); 
            return player != null;
        }

        internal static void Remove(ReferenceHub hub)
            => Cache.Remove(hub);
    }
}
