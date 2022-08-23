using CombatServiceAPI.Characters;
using System;
using System.Collections.Generic;

namespace CombatServiceAPI.Models
{
    [Serializable]
    public class GetBattleInput
    {
        public List<Character> userCharacters { get; set; }
        public List<Character> opponentCharacters { get; set; }
    }
}
