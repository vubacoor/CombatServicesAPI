using CombatServiceAPI.Passive.Models;

namespace CombatServiceAPI.Models
{
    public class RemainEffect
    {
        public Effect effect { get; set; }
        public CombatStat tempStat { get; set; }
        public int expireFor { get; set; }
        public RemainEffect() { }
        public RemainEffect(Effect effect, CombatStat tempStat, int expireFor)
        {
            this.effect = effect;
            this.tempStat = tempStat;
            this.expireFor = expireFor;
        }
    }
}
