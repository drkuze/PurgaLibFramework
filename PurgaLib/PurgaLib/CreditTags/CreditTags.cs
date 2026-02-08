using PurgaLib.API.Enums;
using PurgaLib.API.Features.PluginManager;
using PurgaLib.API.Features.Server;
using PurgaLib.Events.EventArgs.Player;
using PurgaLib.Events.Handlers;
using System;
using System.Collections.Generic;
using PurgaLib.API.Features.HintSystem;


namespace PurgaLib.CreditTags
{
    public class CreditTags : Plugin<Config>
    {
        public override string Name { get; } = "CreditTags";
        public override string Author { get; } = "PurgaLib Team";
        public override string Description { get; } = null;
        public override Version Version { get; } = new Version(Loader.Loader.ApiVersion);
        public override Version RequiredPurgaLibVersion { get; } = new Version(Loader.Loader.ApiVersion);

        protected override void OnEnabled()
        {
            AttachHandler();
        }

        protected override void OnDisabled()
        {
            UnattachHandler();
        }

        private void AttachHandler() => PlayerHandlers.Joined += OnPlayerVerify;

        private void UnattachHandler() => PlayerHandlers.Joined -= OnPlayerVerify;

        public void OnPlayerVerify(PlayerJoinedEventArgs ev)
        {
            if (CreditUsers.TryGetValue(ev.Player.UserId, out var badge))
            {
                Logged.Info($"[PurgaLib] {ev.Player.Nickname} is a PurgaLib Contributor!");
                #pragma warning disable CS0618
                ev.Player.RankName(badge.Text);
                ev.Player.RankColor(badge.Color);
                #pragma warning restore CS0618
                var hint1 = new HintElement(
                    text: "<color=purple>You are a PurgaLib Contributor!</color>",
                    duration: 5f,
                    id: "hint1",
                    zone: HintZone.Bottom
                );
                ev.Player.ShowCustomHint(hint1);
                HintService.Get(ev.Player).Tick();
            }
        }
        
        public class CreditBadge
        {
            public string Text { get; set; }
            public RankColorsType Color { get; set; }
        }

        public Dictionary<string, CreditBadge> CreditUsers { get; set; } = new Dictionary<string, CreditBadge>
        {
            {
                "76561199548842223@steam",
                new CreditBadge
                {
                    Text = GetBadgeText(CreditTagsBadgeNameType.Owner),
                    Color = RankColorsType.deep_pink
                }
            },
            {
                "76561199555131749@steam",
                new CreditBadge
                {
                    Text = GetBadgeText(CreditTagsBadgeNameType.Developer),
                    Color = RankColorsType.aqua
                }
            }
        };


        public enum CreditTagsBadgeNameType
        {
            Owner,
            Developer,
            Contributor
        }

        public static string GetBadgeText(CreditTagsBadgeNameType badge)
        {
            return badge switch
            {
                CreditTagsBadgeNameType.Owner => "👑 PurgaLib Owner 👑",
                CreditTagsBadgeNameType.Developer => "🔧 PurgaLib Developer 🔧",
                CreditTagsBadgeNameType.Contributor => "🌐 PurgaLib Contributor 🌐",
                _ => string.Empty
            };
        }
    }
}