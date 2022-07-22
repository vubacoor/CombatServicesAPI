namespace CombatServiceAPI.Characters
{
    public class DecreaseDamageActions : CharacterActionsDecorator
    {
        public DecreaseDamageActions(ICharacterActions actions) : base(actions)
        {
        }

        public override int NormalFight(Character character, Character target)
        {
            return base.NormalFight(character, target) + (character.atk * 10 / 100);
        }

        public override int SpecialFight(Character character, Character target)
        {
            return base.SpecialFight(character, target) + (character.atk * 10 / 100);
        }
    }
}
