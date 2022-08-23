using System.Collections.Generic;
using CombatServiceAPI.Passive.Models;

namespace CombatServiceAPI.Models
{
    public class CombatOrder
    {
        public int orderNo { get; set; }
        public List<EffectOutput> effectOutputs { get; set; }

        public Dictionary<string, CombatStat> characterStats { get; set; }

        public CombatOrder(int orderNo, List<EffectOutput> effectOutputs)
        {
            this.orderNo = orderNo;
            this.effectOutputs = effectOutputs;
        }
    }
}
