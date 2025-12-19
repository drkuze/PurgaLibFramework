using LabApi.Features.Wrappers;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.SCP;

public class ScpUsingAbilityEventArgs : System.EventArgs
{
    public LabApi.Features.Wrappers.Player Scp { get; }
    public string Ability { get; }

    public ScpUsingAbilityEventArgs(LabApi.Features.Wrappers.Player scp, string ability)
    {
        Scp = scp;
        Ability = ability;
    }
}