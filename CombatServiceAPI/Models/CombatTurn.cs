using System.Collections.Generic;

namespace CombatServiceAPI.Models
{
    public class CombatTurn
    {
        public int turn { get; set; }
        public StartTurn startTurn { get; set; }
        public List<CombatOrder> orders { get; set; }

        public CombatTurn() { }
        public CombatTurn(int turn, StartTurn _startTurn, List<CombatOrder> orders)
        {
            this.turn = turn;
            this.startTurn = _startTurn;
            this.orders = orders;
        }
    }
}
