using CombatServiceAPI.Passive.Interfaces;
using CombatServiceAPI.Characters;
using CombatServiceAPI.Passive.Models;

namespace CombatServiceAPI.Passive.Decorators
{
    public abstract class PassiveDecorator : IPassiveLogic
    {
        private IPassiveLogic _passive;


        protected PassiveDecorator(IPassiveLogic passive)
        {
            _passive = passive;
        }

        public virtual CombatStat CalculateStat(CombatStat combatStat, int turn)
        {
            return _passive.CalculateStat(combatStat, turn);
        }
    }
}
