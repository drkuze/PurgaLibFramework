namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player;

public class PlayerHurtingEventArgs : System.EventArgs
{
    public PurgaLibAPI.Features.Player Attacker { get; }
    public PurgaLibAPI.Features.Player Target { get; }
    public float Damage { get; set; }
    public bool IsAllowed { get; set; } = true;

    public PlayerHurtingEventArgs(PurgaLibAPI.Features.Player attacker, PurgaLibAPI.Features.Player target, float damage)
    {
        Attacker = attacker;
        Target = target;
        Damage = damage;
    }
}