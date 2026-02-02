using PurgaLib.Events.EventSystem.Interfaces;

namespace PurgaLib.Events.EventArgs.Player;

public class PlayerHurtingEventArgs : IEventArgs
{
    public global::PurgaLib.API.Features.Player Attacker { get; }
    public global::PurgaLib.API.Features.Player Target { get; }
    public float Damage { get; set; }
    public bool IsAllowed { get; set; } = true;

    public PlayerHurtingEventArgs(global::PurgaLib.API.Features.Player attacker, global::PurgaLib.API.Features.Player target, float damage)
    {
        Attacker = attacker;
        Target = target;
        Damage = damage;
    }
}