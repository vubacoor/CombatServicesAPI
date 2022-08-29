using System.Collections.Generic;
using CombatServiceAPI.Models;

namespace CombatServiceAPI.Model
{
    public class BattleData
    {
        public bool skip { get; set; }
        public int status { get; set; }
        public List<CombatTurn> battleProgress { get; set; }
    }
}
