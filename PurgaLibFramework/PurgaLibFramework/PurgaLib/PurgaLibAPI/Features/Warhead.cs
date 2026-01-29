using System.Collections.Generic;
using Interactables.Interobjects.DoorUtils;
using UnityEngine;
using Mirror;
using MapGeneration;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features
{
    /// <summary>
    /// Gestione semplificata della Warhead.
    /// </summary>
    public static class Warhead
    {
        /// <summary>
        /// Ottiene il controller singleton del warhead.
        /// </summary>
        public static AlphaWarheadController Controller => AlphaWarheadController.Singleton;

        /// <summary>
        /// Ottiene il pannello esterno del warhead.
        /// </summary>
        private static AlphaWarheadOutsitePanel OutsitePanel =>
            Object.FindObjectOfType<AlphaWarheadOutsitePanel>();

        /// <summary>
        /// Ottiene il pannello interno del warhead.
        /// </summary>
        private static AlphaWarheadNukesitePanel SitePanel => AlphaWarheadOutsitePanel.nukeside;

        /// <summary>
        /// Ottiene o imposta lo stato automatico di detonazione.
        /// </summary>
        public static bool AutoDetonate
        {
            get => Controller._autoDetonate;
            set => Controller._autoDetonate = value;
        }

        /// <summary>
        /// Stato del warhead.
        /// </summary>
        public static bool LeverStatus
        {
            get => SitePanel.Networkenabled;
            set => SitePanel.Networkenabled = value;
        }
        

        /// <summary>
        /// Controlla se il warhead è in corso.
        /// </summary>
        public static bool IsInProgress => Controller.Info.InProgress;

        /// <summary>
        /// Controlla se il warhead è già detonata.
        /// </summary>
        public static bool IsDetonated => Controller.AlreadyDetonated;

        /// <summary>
        /// Timer rimanente alla detonazione.
        /// </summary>
        public static float DetonationTimer
        {
            get => AlphaWarheadController.TimeUntilDetonation;
            set => Controller.ForceTime(value);
        }

        /// <summary>
        /// Timer reale della detonazione.
        /// </summary>
        public static float RealDetonationTimer => Controller.CurScenario.TimeToDetonate;

        /// <summary>
        /// Se il warhead è bloccata (non può essere avviata).
        /// </summary>
        public static bool IsLocked
        {
            get => Controller.IsLocked;
            set => Controller.IsLocked = value;
        }

        /// <summary>
        /// Kills causati dalla detonazione.
        /// </summary>
        public static int Kills
        {
            get => Controller.WarheadKills;
            set => Controller.WarheadKills = value;
        }

        /// <summary>
        /// Controlla se il warhead può essere avviata.
        /// </summary>
        public static bool CanBeStarted => !IsInProgress && !IsDetonated && Controller.CooldownEndTime <= NetworkTime.time;

        /// <summary>
        /// Inizia il warhead countdown.
        /// </summary>
        public static void Start()
        {
            Controller.InstantPrepare();
            Controller.StartDetonation(false);
        }

        /// <summary>
        /// Ferma il warhead.
        /// </summary>
        public static void Stop() => Controller.CancelDetonation();

        /// <summary>
        /// Detona il warhead immediatamente.
        /// </summary>
        public static void Detonate() => Controller.ForceTime(0f);

        /// <summary>
        /// Scuoti tutti i giocatori, come se fosse detonata.
        /// </summary>
        public static void Shake() => Controller.RpcShake(false);

        /// <summary>
        /// Apri o chiudi tutte le porte del warhead.
        /// </summary>
        public static void TriggerDoors(bool open) =>
            DoorEventOpenerExtension.TriggerAction(open 
                ? DoorEventOpenerExtension.OpenerEventType.WarheadStart 
                : DoorEventOpenerExtension.OpenerEventType.WarheadCancel);

        /// <summary>
        /// Chiude tutte le blast doors.
        /// </summary>
        public static void CloseBlastDoors()
        {
            foreach (var door in BlastDoors)
                door._isOpen = false;
        }

        /// <summary>
        /// Tutte le blast doors.
        /// </summary>
        public static IReadOnlyCollection<BlastDoor> BlastDoors => BlastDoor.Instances;

        /// <summary>
        /// Controlla se una posizione può essere colpita dalla detonazione.
        /// </summary>
        public static bool CanBeDetonated(Vector3 pos, bool includeOnlyLifts = false) =>
            AlphaWarheadController.CanBeDetonated(pos, includeOnlyLifts);
    }
}
