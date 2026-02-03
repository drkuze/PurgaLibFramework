using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MapGeneration.Distributors;
using Footprinting;
using PurgaLib.API.Enums;

namespace PurgaLib.API.Features
{
    public class Generator
    {
        internal static List<Scp079Generator> BaseGenerators { get; set; } = new();

        public static IEnumerable<Generator> List => BaseGenerators.Select(g => new Generator(g));

        internal Scp079Generator Base { get; }

        internal Generator(Scp079Generator generator)
        {
            Base = generator;
            if (!BaseGenerators.Contains(generator))
                BaseGenerators.Add(generator);
        }

        public static Generator Get(int index) => new(BaseGenerators[index]);
        public static Generator Get(Scp079Generator generator) => new(generator);

        public int Id => Base.GetInstanceID();
        public bool IsEngaged => Base.Engaged;
        public bool IsLocked => !Base.IsUnlocked;
        public bool IsOpen => Base.IsOpen;
        public bool IsActivating => Base.Activating;

        public Vector3 Position => Base.transform.position;
        public Transform Transform => Base.transform;
        public GameObject GameObject => Base.gameObject;

        public Footprint? LastActivator
        {
            get => Base._lastActivator;
            set => Base._lastActivator = value ?? new Footprint();
        }
        
        public void Engage() => Base.Engaged = true;
        public void Disengage() => Base.Engaged = false;
        public void Lock() => Base.IsUnlocked = false;
        public void Unlock() => Base.IsUnlocked = true;
        public void Activate()
        {
            Base.Activating = true;
            Base._leverStopwatch.Restart();
        }
        public void Deactivate()
        {
            Base.Activating = false;
            Base._lastActivator = new Footprint();
        }

        public float TotalActivationTime
        {
            get => Base.TotalActivationTime;
            set => Base.TotalActivationTime = value;
        }

        public float TotalDeactivationTime
        {
            get => Base.TotalDeactivationTime;
            set => Base.TotalDeactivationTime = value;
        }

        public static IEnumerable<Generator> Get(GeneratorState state)
        {
            return state switch
            {
                GeneratorState.Engaged => BaseGenerators.Where(g => g.Engaged).Select(g => new Generator(g)),
                GeneratorState.Disengaged => BaseGenerators.Where(g => !g.Engaged).Select(g => new Generator(g)),
                _ => BaseGenerators.Select(g => new Generator(g)),
            };
        }

        public static void RefreshAll()
        {
            foreach (var gen in BaseGenerators)
                gen.ServerUpdate();
        }

        public override string ToString() =>
            $"Generator {Id} | Engaged: {IsEngaged} | Locked: {IsLocked} | Activating: {IsActivating}";
    }
}
