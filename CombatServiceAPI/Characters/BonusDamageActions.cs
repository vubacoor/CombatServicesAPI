namespace CombatServiceAPI.Characters
{
    public class BonusDamageActions : CharacterActionsDecorator
    {
        public BonusDamageActions(ICharacterActions actions) : base(actions)
        {
        }

        public override int NormalFight(Character character, Character target)
        {
            return base.NormalFight(character, target) - (character.atk * 10 / 100);
        }

        public override int SpecialFight(Character character, Character target)
        {
            return base.SpecialFight(character, target) - (character.atk * 10 / 100);
        }
    }
}
