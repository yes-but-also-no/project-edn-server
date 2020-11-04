using Data.Configuration.Poo;
using Data.Model.Items;
using GameServer.Model.Units;

namespace GameServer.Model.Parts.Body
{
    public class Arms : Part
    {
        /// <summary>
        /// Stats for this part
        /// </summary>
        private new ArmStats Stats => base.Stats as ArmStats;

        /// <summary>
        /// The maximum overheat these arms can sustain
        /// </summary>
        public float MaxOverheat => Stats.Endurance;
        
        /// <summary>
        /// The overheat recovery per second these arms provide
        /// </summary>
        public float OverheatRecovery => Stats.Recovery;
        
        public Arms(PartRecord partRecord, Unit owner) : base(partRecord, owner)
        {
        }
    }
}