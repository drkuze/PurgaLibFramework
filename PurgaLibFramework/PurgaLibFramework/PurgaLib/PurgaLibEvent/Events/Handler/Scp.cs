using System;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.SCP;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Handler
{
    public static class Scp
    {
        public static event EventHandler<ScpUsingAbilityEventArgs> UsingAbility;
        public static event EventHandler<ScpAttackingEventArgs> Attacking;
        
        internal static void OnAttacking(ScpAttackingEventArgs ev)
            => EventManager.Invoke(Attacking, null, ev);
        
        internal static void OnUsingAbility(ScpUsingAbilityEventArgs ev)
            => EventManager.Invoke(UsingAbility, null, ev);
    }
}