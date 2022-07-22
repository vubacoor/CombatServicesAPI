using System.Collections.Generic;

namespace CombatServiceAPI.Model
{
    public class BattleProgess
    {
        public BattleUnit attacker { get; set; }
        public List<BattleUnit> target { get; set; }
        public int turn { get; set; }
        public int order { get; set; }
        public string type { get; set; }
    }
}
