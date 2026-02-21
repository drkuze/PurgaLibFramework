using Respawning.Objectives;

namespace PurgaLib.API.Features.Objectives
{
    public class HumanObjective<T> : Objective
        where T : ObjectiveFootprintBase, new()
    {
        public new HumanObjectiveBase<T> Base { get; }

        public HumanObjective(HumanObjectiveBase<T> baseObjective)
            : base(baseObjective)
        {
            Base = baseObjective;
        }
        
        public T ObjectiveFootprint
        {
            get => (T)Base.ObjectiveFootprint;
            set => Base.ObjectiveFootprint = value;
        }
        
        public float TimeReward
        {
            get => ObjectiveFootprint?.TimeReward ?? 0;
            set
            {
                ObjectiveFootprint ??= new T();
                ObjectiveFootprint.TimeReward = value;
            }
        }
        
        public float InfluenceReward
        {
            get => ObjectiveFootprint?.InfluenceReward ?? 0;
            set
            {
                ObjectiveFootprint ??= new T();
                ObjectiveFootprint.InfluenceReward = value;
            }
        }
        
        public Player Achiever
        {
            get => ObjectiveFootprint == null ? null : Player.Get(ObjectiveFootprint.AchievingPlayer.Nickname);
            set
            {
                ObjectiveFootprint ??= new T();
                ObjectiveFootprint.AchievingPlayer = new(value.Footprint);
            }
        }
        
        public void Achieve(T footprint)
        {
            ObjectiveFootprint = footprint;
            Achieve();
        }
    }
}