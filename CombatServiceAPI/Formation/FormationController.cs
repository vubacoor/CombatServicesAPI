using CombatServiceAPI.Characters;
using System.Collections.Generic;

namespace CombatServiceAPI.Formation
{
    public class FormationController
    {
        public List<BaseCharacter> userCharacters;
        public List<BaseCharacter> opponentCharacters;

        public FormationController()
        {
            userCharacters = new List<BaseCharacter>();
            opponentCharacters = new List<BaseCharacter>();
        }
        public void AddCharacter(BaseCharacter character, string side)
        {
            switch (character.element)
            {
                case "Anima":
                    Anima animaChar = (Anima)character;
                    if (side == "user")
                    {
                        userCharacters.Add(animaChar);
                    }
                    else
                    {
                        opponentCharacters.Add(animaChar);
                    }
                    break;
                case "Aqua":
                    Aqua aquaChar = (Aqua)character;
                    if (side == "user")
                    {
                        userCharacters.Add(aquaChar);
                    }
                    else
                    {
                        opponentCharacters.Add(aquaChar);
                    }
                    break;
                case "Earth":
                    Earth earthChar = (Earth)character;
                    if (side == "user")
                    {
                        userCharacters.Add(earthChar);
                    }
                    else
                    {
                        opponentCharacters.Add(earthChar);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
