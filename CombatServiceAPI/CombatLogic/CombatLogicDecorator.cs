namespace CombatServiceAPI.Characters
{
    public abstract class CombatLogicDecorator : ICombatLogic
    {
        private ICombatLogic _actions;


        protected CombatLogicDecorator(ICombatLogic actions)
        {
            _actions = actions;
        }

        public virtual int CalculateDamage(int baseDamage)
        {
            return _actions.CalculateDamage(baseDamage);
        }
    }
}
