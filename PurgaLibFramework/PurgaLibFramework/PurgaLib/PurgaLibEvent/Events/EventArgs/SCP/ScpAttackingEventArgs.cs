using LabApi.Features.Wrappers;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.SCP;

public class ScpAttackingEventArgs : System.EventArgs
{
    public LabApi.Features.Wrappers.Player Scp { get; }
    public LabApi.Features.Wrappers.Player Target { get; }
    public bool IsAllowed { get; set; } = true;

    public ScpAttackingEventArgs(LabApi.Features.Wrappers.Player scp, LabApi.Features.Wrappers.Player target)
    {
        Scp = scp;
        Target = target;
    }
}