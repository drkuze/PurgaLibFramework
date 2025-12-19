namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player;

public class PlayerHurtingEventArgs : System.EventArgs
{
    public LabApi.Features.Wrappers.Player Attacker { get; }
    public LabApi.Features.Wrappers.Player Target { get; }
    public float Damage { get; set; }
    public bool IsAllowed { get; set; } = true;

    public PlayerHurtingEventArgs(LabApi.Features.Wrappers.Player attacker, LabApi.Features.Wrappers.Player target, float damage)
    {
        Attacker = attacker;
        Target = target;
        Damage = damage;
    }
}