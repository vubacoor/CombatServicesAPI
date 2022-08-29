using CombatServiceAPI.Passive.Models;
using System.Collections.Generic;

namespace CombatServiceAPI.Models
{
    public class StartTurn
    {
        public List<EffectOutput> effects { get; set; }
        public Dictionary<string, CombatStat> characterStats { get; set; }


        public StartTurn()
        {
        }
        public StartTurn(List<EffectOutput> effects, Dictionary<string, CombatStat> characterStats)
        {
            this.effects = effects;
            this.characterStats = characterStats;
        }
    }
}
