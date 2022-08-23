using System;
using CombatServiceAPI.Passive.Interfaces;
using CombatServiceAPI.Characters;
using CombatServiceAPI.Passive.Models;

namespace CombatServiceAPI.Passive.Base
{
    public class PassiveLogic : IPassiveLogic
    {
        public CombatStat CalculateStat(CombatStat combatStat, int turn)
        {
            return combatStat;
        }
    }
}
