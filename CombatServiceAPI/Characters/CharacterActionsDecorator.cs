namespace CombatServiceAPI.Characters
{
    public abstract class CharacterActionsDecorator : ICharacterActions
    {
        private ICharacterActions _actions;


        protected CharacterActionsDecorator(ICharacterActions actions)
        {
            _actions = actions;
        }

        public virtual int NormalFight(Character character, Character target)
        {
            return _actions.NormalFight(character, target);
        }

        public virtual int SpecialFight(Character character, Character target)
        {
            return _actions.SpecialFight(character, target);
        }
    }
}
