using System.Collections.Generic;

namespace CombatServiceAPI.Models
{
    public class CombatTurn
    {
        public int turn { get; set; }
        public List<EffectOutput> startTurnEffects { get; set; }
        public List<CombatOrder> orders { get; set; }

        public CombatTurn() { }
        public CombatTurn(int turn, List<EffectOutput> startTurnEffects, List<CombatOrder> orders)
        {
            this.turn = turn;
            this.startTurnEffects = startTurnEffects;
            this.orders = orders;
        }
    }
}
