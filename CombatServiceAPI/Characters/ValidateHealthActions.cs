namespace CombatServiceAPI.Characters
{
    public class ValidateHealthActions : CharacterActionsDecorator
    {
        public ValidateHealthActions(ICharacterActions actions) : base(actions)
        {
        }

        public override int NormalFight(Character character, Character target)
        {
            if (base.NormalFight(character, target) < 0)
            {
                return 0;
            }
            else
            {
                return base.NormalFight(character, target) - (character.atk * 10 / 100);
            }
        }

        public override int SpecialFight(Character character, Character target)
        {
            if (base.SpecialFight(character, target) < 0)
            {
                return 0;
            }
            else
            {
                return base.SpecialFight(character, target) - (character.atk * 10 / 100);
            }
        }
    }
}
