using CombatServiceAPI.Characters;
using CombatServiceAPI.Passive.Models;
using System;
using System.Collections.Generic;

namespace CombatServiceAPI.Services
{
    public static class StatCalculator
    {
        public static CombatStat GetDeviantStats(CombatStat baseStat, Effect effect)
        {
            CombatStat deviantStats = new CombatStat(0, 0, 0, 0, 0, 0, 0, 0);
            string amtString = Convert.ToString(effect.amount);
            float amtPerRariry;
            if (amtString.Contains("|"))
            {
                amtPerRariry = float.Parse(effect.amount.ToString().Split("|")[0]);
            }
            else
            {
                amtPerRariry = float.Parse(amtString);
            }
            if (effect.amountType == "PERCENT")
            {
                switch (effect.statEffect)
                {
                    case StatEffect.HP_TARGET:
                    case StatEffect.HP_OWNER:
                        deviantStats.hp += baseStat.hp * (amtPerRariry / 100f);
                        break;
                    case StatEffect.SPD_TARGET:
                    case StatEffect.SPD_OWNER:
                        deviantStats.speed += baseStat.speed * (amtPerRariry / 100f);
                        break;
                    case StatEffect.ATK_TARGET:
                    case StatEffect.ATK_OWNER:
                        deviantStats.atk += baseStat.atk * (amtPerRariry / 100f);
                        break;
                    case StatEffect.DEF_TARGET:
                    case StatEffect.DEF_OWNER:
                        deviantStats.def += baseStat.def * (amtPerRariry / 100f);
                        break;
                    case StatEffect.DAMAGE_TARGET:
                    case StatEffect.DAMAGE_OWNER:
                        deviantStats.atk += baseStat.atk * (amtPerRariry / 100f);
                        break;
                }
            }
            else
            {
                switch (effect.statEffect)
                {
                    case StatEffect.HP_TARGET:
                    case StatEffect.HP_OWNER:
                        deviantStats.hp += amtPerRariry;
                        break;
                    case StatEffect.SPD_TARGET:
                    case StatEffect.SPD_OWNER:
                        deviantStats.speed += amtPerRariry;
                        break;
                    case StatEffect.ATK_TARGET:
                    case StatEffect.ATK_OWNER:
                        deviantStats.atk += amtPerRariry;
                        break;
                    case StatEffect.DEF_TARGET:
                    case StatEffect.DEF_OWNER:
                        deviantStats.def += amtPerRariry;
                        break;
                    case StatEffect.DAMAGE_TARGET:
                    case StatEffect.DAMAGE_OWNER:
                        deviantStats.atk += amtPerRariry;
                        break;
                }
            }
            return deviantStats;
        }

        public static float GetRecoverAmt(CombatStat baseStat, Effect effect)
        {
            float recoverAmt = 0;
            string amtString = Convert.ToString(effect.amount);
            float amtPerRariry;
            if (amtString.Contains("|"))
            {
                amtPerRariry = float.Parse(effect.amount.ToString().Split("|")[0]);
            }
            else
            {
                amtPerRariry = float.Parse(amtString);
            }
            if (effect.amountType == "PERCENT")
            {
                switch (effect.statEffect)
                {
                    case StatEffect.HP_OWNER:
                        recoverAmt = baseStat.hp * (amtPerRariry / 100f);
                        break;
                    case StatEffect.SPD_OWNER:
                        recoverAmt = baseStat.speed * (amtPerRariry / 100f);
                        break;
                    case StatEffect.ATK_OWNER:
                        recoverAmt = baseStat.atk * (amtPerRariry / 100f);
                        break;
                    case StatEffect.DEF_OWNER:
                        recoverAmt = baseStat.def * (amtPerRariry / 100f);
                        break;
                    case StatEffect.DAMAGE_OWNER:
                        recoverAmt = baseStat.atk * (amtPerRariry / 100f);
                        break;
                }
            }
            else
            {
                switch (effect.statEffect)
                {
                    case StatEffect.HP_OWNER:
                        recoverAmt = amtPerRariry;
                        break;
                    case StatEffect.SPD_OWNER:
                        recoverAmt = amtPerRariry;
                        break;
                    case StatEffect.ATK_OWNER:
                        recoverAmt = amtPerRariry;
                        break;
                    case StatEffect.DEF_OWNER:
                        recoverAmt = amtPerRariry;
                        break;
                    case StatEffect.DAMAGE_OWNER:
                        recoverAmt = amtPerRariry;
                        break;
                }
            }
            return recoverAmt;
        }
    }
}
