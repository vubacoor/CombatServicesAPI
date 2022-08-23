using CombatServiceAPI.Passive.Models;
using CombatServiceAPI.Passive.Interfaces;
using CombatServiceAPI.Characters;
using CombatServiceAPI.Modules;
using System;

namespace CombatServiceAPI.Passive.Decorators
{
    public class ModifyFlatPassive : PassiveDecorator
    {
        private readonly Effect effect;
        public ModifyFlatPassive(IPassiveLogic actions, Effect effect) : base(actions)
        {
            this.effect = effect;
        }

        public override CombatStat CalculateStat(CombatStat combatStat, int turn)
        {
            bool canTrigger = BattleLogic.CheckIfCanTriggerEffect(effect.cost, effect.rate, turn, effect.stackable);
            if (canTrigger)
            {
                HandleCalculateStat(combatStat, turn);
            }
            else
            {
                combatStat = base.CalculateStat(combatStat, turn);
            }
            return combatStat;
        }

        public void HandleCalculateStat(CombatStat combatStat, int turn)
        {
            string amtString = Convert.ToString(effect.amount);
            int amtPerRariry;
            if (amtString.Contains("|"))
            {
                amtPerRariry = Int32.Parse(effect.amount.ToString().Split("|")[0]);
            }
            else
            {
                amtPerRariry = Int32.Parse(amtString);
            }
            switch (effect.statEffect)
            {
                case StatEffect.HP_OWNER:
                    base.CalculateStat(combatStat, turn).hp += amtPerRariry;
                    break;
                case StatEffect.SPD_OWNER:
                    base.CalculateStat(combatStat, turn).speed += amtPerRariry;
                    break;
                case StatEffect.ATK_OWNER:
                    base.CalculateStat(combatStat, turn).atk += amtPerRariry;
                    break;
                case StatEffect.DEF_OWNER:
                    base.CalculateStat(combatStat, turn).def += amtPerRariry;
                    break;
            }
        }
    }
}
