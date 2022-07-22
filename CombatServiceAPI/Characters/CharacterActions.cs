namespace CombatServiceAPI.Characters
{
    public class CharacterActions : ICharacterActions
    {
        public int NormalFight(Character character, Character target)
        {
            return target.hp - character.atk;
        }

        public int SpecialFight(Character character, Character target)
        {
            return target.hp - character.atk;
        }
    }
}
