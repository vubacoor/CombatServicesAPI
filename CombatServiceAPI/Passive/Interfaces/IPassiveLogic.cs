using CombatServiceAPI.Characters;
using CombatServiceAPI.Passive.Models;

namespace CombatServiceAPI.Passive.Interfaces
{
    public interface IPassiveLogic
    {
        CombatStat CalculateStat(CombatStat combatStat, int turn);
    }
}
