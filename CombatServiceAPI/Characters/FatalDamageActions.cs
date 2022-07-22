namespace CombatServiceAPI.Characters
{
    public class FatalDamageActions : CharacterActionsDecorator
    {
        public FatalDamageActions(ICharacterActions actions) : base(actions)
        {
        }

        public override int NormalFight(Character character, Character target)
        {
            return base.NormalFight(character, target) * 0;
        }

        public override int SpecialFight(Character character, Character target)
        {
            return base.SpecialFight(character, target) * 0;
        }
    }
}
