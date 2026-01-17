namespace PurgaLibEvents.PurgaLibEvent.Events.EventArgs.Player;

public class PlayerHurtingEventArgs : System.EventArgs
{
    public PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player Attacker { get; }
    public PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player Target { get; }
    public float Damage { get; set; }
    public bool IsAllowed { get; set; } = true;

    public PlayerHurtingEventArgs(PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player attacker, PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player target, float damage)
    {
        Attacker = attacker;
        Target = target;
        Damage = damage;
    }
}