using System;
using GameCore;
using RoundRestarting;

namespace PurgaLib.API.Features.Server;

public static class Round
{
    public static TimeSpan ElapsedTime => RoundStart.RoundLength;
    public static DateTime StartedTime => DateTime.Now - ElapsedTime;
    public static bool IsStarted => ReferenceHub.TryGetHostHub(out ReferenceHub hub) && hub.characterClassManager.RoundStarted;
    public static bool InProgress => !IsEnded && RoundSummary.RoundInProgress();
    public static bool IsEnded => RoundSummary.singleton.IsRoundEnded && RoundSummary.singleton.IsRoundEnded;
    public static bool IsLobby => !(IsEnded || IsStarted);
    
    public static RoundSummary.SumInfo_ClassList LastClassList { get; internal set; }
    public static bool IsLocked
    {
        get => RoundSummary.RoundLock;
        set => RoundSummary.RoundLock = value;
    }
    public static bool IsLobbyLocked
    {
        get => RoundStart.LobbyLock;
        set => RoundStart.LobbyLock = value;
    }
    public static short LobbyWaitingTime
    {
        get => RoundStart.singleton.NetworkTimer;
        set => RoundStart.singleton.NetworkTimer = value;
    }
    public static void Restart(bool fastRestart = true, bool overrideRestartAction = false, ServerStatic.NextRoundAction restartAction = ServerStatic.NextRoundAction.DoNothing)
    {
        if (overrideRestartAction)
            ServerStatic.StopNextRound = restartAction;
        bool oldValue = CustomNetworkManager.EnableFastRestart;
        CustomNetworkManager.EnableFastRestart = fastRestart;

        RoundRestart.InitiateRoundRestart();

        CustomNetworkManager.EnableFastRestart = oldValue;
    }
        
    public static bool End(bool End = false)
    {
        if (RoundSummary.singleton.KeepRoundOnOne && Player.List.Count < 2 && !End)
            return false;

        if ((IsStarted && !IsLocked) || End)
        {
            RoundSummary.singleton.ForceEnd();
            return true;
        }

        return false;
    }
        
    public static void Start() => CharacterClassManager.ForceRoundStart();
}