using System.Collections.Generic;
using CombatServiceAPI.Passive.Models;

namespace CombatServiceAPI.Models
{
    public class CombatOrder
    {
        public int orderNo { get; set; }

        public string characterId { get; set; }
        public List<EffectOutput> actionEffects { get; set; }
        public List<EffectOutput> endOrderEffects { get; set; }

        public Dictionary<string, CombatStat> characterStats { get; set; }

        public CombatOrder(int orderNo, string characterId, List<EffectOutput> actionEffects, List<EffectOutput> endOrderEffects)
        {
            this.orderNo = orderNo;
            this.characterId = characterId;
            this.actionEffects = actionEffects;
            this.endOrderEffects = endOrderEffects;
        }
    }
}
