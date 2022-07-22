using System.Collections.Generic;

namespace CombatServiceAPI.Model
{
    public class BattleData
    {
        public bool skip { get; set; }
        public int status { get; set; }
        public List<BattleProgess> battleProgress { get; set; }
    }
}
