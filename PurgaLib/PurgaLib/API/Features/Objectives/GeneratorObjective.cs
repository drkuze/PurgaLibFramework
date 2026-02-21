using PurgaLib.API.Enums;

namespace PurgaLib.API.Features.Objectives
{
    using BaseObjective = Respawning.Objectives.GeneratorActivatedObjective;

    public sealed class GeneratorObjective : Objective
    {
        public new BaseObjective Base { get; }

        public override ObjectiveType Type => ObjectiveType.GeneratorActivation;

        internal GeneratorObjective(BaseObjective baseObjective)
            : base(baseObjective)
        {
            Base = baseObjective;
        }
        
        public void Activate(Generator generator, Player player = null)
        {
            if (generator == null) return;

            Base.OnGeneratorEngaged(generator.Base, (player ?? Server.Features.Host).Footprint);
        }
    }
}