using CombatServiceAPI.Characters;
using System.Collections.Generic;

namespace CombatServiceAPI.Models
{
    public class GetBattleInput
    {
        public List<BaseCharacter> userCharacters { get; set; }
        public List<BaseCharacter> opponentCharacters { get; set; }
    }
}
