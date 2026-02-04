using System.Collections.Generic;
using System.Linq;
using PlayerRoles.PlayableScps.Scp079;
using PurgaLib.API.Enums;
using UnityEngine;

namespace PurgaLib.API.Features
{
    public static class Recontainer
    {
        internal static Scp079Recontainer Base { get; set; }

        public static IEnumerable<Door> ContainmentGates =>
            Door.List.Where(d => Base._containmentGates.Contains(d.Base));

        public static int EngagedGeneratorCount => Base._prevEngaged;
        public static bool IsContainmentZoneOpen
        {
            get => ContainmentGates.All(d => d.IsOpen);
            set => Base.SetContainmentDoors(value, IsContainmentZoneLocked);
        }
        public static bool IsContainmentZoneLocked
        {
            get => ContainmentGates.All(d => d.IsLocked);
            set => Base.SetContainmentDoors(IsContainmentZoneOpen, value);
        }

        public static float LockdownDuration
        {
            get => Base._lockdownDuration;
            set => Base._lockdownDuration = value;
        }

        public static GameObject ActivatorButton => Base._activatorButton.gameObject;
        public static Vector3 ActivatorButtonPosition
        {
            get => ActivatorButton.transform.localPosition;
            set => ActivatorButton.transform.localPosition = value;
        }

        public static Window ActivatorWindow => Window.Get(Base._activatorGlass);
        public static Vector3 ActivatorPosition => Base._activatorPos;

        public static float ActivatorLerpSpeed
        {
            get => Base._activatorLerpSpeed;
            set => Base._activatorLerpSpeed = value;
        }

        public static string ProgressAnnouncement
        {
            get => Base._announcementProgress;
            set => Base._announcementProgress = value;
        }
        public static string CountdownAnnouncement
        {
            get => Base._announcementCountdown;
            set => Base._announcementCountdown = value;
        }
        public static string ContainmentSuccessAnnouncement
        {
            get => Base._announcementSuccess;
            set => Base._announcementSuccess = value;
        }
        public static string ContainmentFailureAnnouncement
        {
            get => Base._announcementFailure;
            set => Base._announcementFailure = value;
        }
        public static string AllGeneratorsActivatedAnnouncement
        {
            get => Base._announcementAllActivated;
            set => Base._announcementAllActivated = value;
        }

        public static bool IsContainmentSequenceDone
        {
            get => Base._alreadyRecontained;
            set => Base._alreadyRecontained = value;
        }
        public static bool IsContainmentSequenceSuccessful
        {
            get => Base._success;
            set => Base._success = value;
        }

        public static IEnumerable<Door> LockedDoors => Base._lockedDoors.Select(Door.Get);

        public static bool TryKillScp079() => Base.TryKill079();

        public static void PlayAnnouncement(string announcement)
            => Base.PlayAnnouncement(announcement, false, false);
        
        public static void BeginOvercharge(bool endOvercharge = true)
        {
            Base.BeginOvercharge();
            if (endOvercharge)
                Base._unlockStopwatch.Start();
        }

        public static void EndOvercharge() => Base.EndOvercharge();

        public static void AnnounceEngagementStatus() 
            => Base.UpdateStatus(Generator.Get(GeneratorState.Engaged).Count());
        public static void AnnounceEngagementStatus(int engagedGenerators) 
            => Base.UpdateStatus(engagedGenerators);

        public static void RefreshEngamentStatus() => Base.RefreshAmount();
        public static void Recontain() => Base.Recontain(false);
        public static void RefreshActivator() => Base.RefreshActivator();
        public static void BreakGlass() => ActivatorWindow.Base.BreakWindow();
    }
}
