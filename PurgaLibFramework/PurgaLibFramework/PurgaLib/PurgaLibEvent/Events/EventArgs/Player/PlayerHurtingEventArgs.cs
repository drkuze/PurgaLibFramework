using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventSystem.Interfaces;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player;

public class PlayerHurtingEventArgs : IEventArgs
{
    public global::PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player Attacker { get; }
    public global::PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player Target { get; }
    public float Damage { get; set; }
    public bool IsAllowed { get; set; } = true;

    public PlayerHurtingEventArgs(global::PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player attacker, global::PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player target, float damage)
    {
        Attacker = attacker;
        Target = target;
        Damage = damage;
    }
}