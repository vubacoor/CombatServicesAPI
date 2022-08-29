using CombatServiceAPI.Passive.Models;

namespace CombatServiceAPI.Models
{
    public class EffectOutput
    {
        public string casterId { get; set; }
        public string targetId { get; set; }
        public string effectBase { get; set; }
        public string statEffect { get; set; }

        public string additionalEffect { get; set; }
        public int expireTo { get; set; }
        public CombatStat targetNewStat { get; set; }
        public EffectOutput() { }
        public EffectOutput(string casterId, string targetId, string effectBase, string statEffect, string additionalEffect, CombatStat targetNewStat)
        {
            this.casterId = casterId;
            this.targetId = targetId;
            this.effectBase = effectBase;
            this.statEffect = statEffect;
            this.additionalEffect = additionalEffect;
            this.targetNewStat = targetNewStat;
        }
        public EffectOutput(string casterId, string targetId, string effectBase, string statEffect, string additionalEffect, int expireTo, CombatStat targetNewStat)
        {
            this.casterId = casterId;
            this.targetId = targetId;
            this.effectBase = effectBase;
            this.statEffect = statEffect;
            this.additionalEffect = additionalEffect;
            this.expireTo = expireTo;
            this.targetNewStat = targetNewStat;
        }
    }
}
