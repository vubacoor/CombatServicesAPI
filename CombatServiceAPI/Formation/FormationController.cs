using CombatServiceAPI.Characters;
using System.Collections.Generic;

namespace CombatServiceAPI.Formation
{
    public class FormationController
    {
        public List<Character> userCharacters;
        public List<Character> opponentCharacters;

        public FormationController()
        {
            userCharacters = new List<Character>();
            opponentCharacters = new List<Character>();
        }
        public void AddCharacter(Character character, string side)
        {
            if (side == "user")
            {
                userCharacters.Add(character);
            }
            else
            {
                opponentCharacters.Add(character);
            }
        }
    }
}
