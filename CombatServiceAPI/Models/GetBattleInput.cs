using CombatServiceAPI.Models;
using System.Collections.Generic;

namespace CombatServiceAPI.Models
{
    public class GetBattleInput
    {
        public List<FormationCharacter> userCharacters { get; set; }
        public List<FormationCharacter> opponentCharacters { get; set; }
    }
}
