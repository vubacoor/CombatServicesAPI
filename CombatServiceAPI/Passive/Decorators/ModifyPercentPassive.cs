using CombatServiceAPI.Passive.Models;
using CombatServiceAPI.Passive.Interfaces;
using CombatServiceAPI.Characters;
using CombatServiceAPI.Modules;
using System;

namespace CombatServiceAPI.Passive.Decorators
{
    public class ModifyPercentPassive : PassiveDecorator
    {
        private readonly Effect effect;
        private readonly BaseStat baseStat;
        public ModifyPercentPassive(IPassiveLogic actions, Effect effect, BaseStat baseStat) : base(actions)
        {
            this.effect = effect;
            this.baseStat = baseStat;
        }

        public override CombatStat CalculateStat(CombatStat combatStat, int turn)
        {
            bool canTrigger = BattleLogic.CheckIfCanTriggerEffect(effect, combatStat, turn);
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
                    base.CalculateStat(combatStat, turn).hp += baseStat.hp / 100 * amtPerRariry;
                    break;
                case StatEffect.SPD_OWNER:
                    base.CalculateStat(combatStat, turn).speed += baseStat.speed / 100 * amtPerRariry;
                    break;
                case StatEffect.ATK_OWNER:
                    base.CalculateStat(combatStat, turn).atk += baseStat.atk / 100 * amtPerRariry;
                    break;
                case StatEffect.DEF_OWNER:
                    base.CalculateStat(combatStat, turn).def += baseStat.def / 100 * amtPerRariry;
                    break;
                case StatEffect.DAMAGE_TARGET:
                    base.CalculateStat(combatStat, turn).shieldAmt = amtPerRariry;
                    break;
            }

        }

    }
}
