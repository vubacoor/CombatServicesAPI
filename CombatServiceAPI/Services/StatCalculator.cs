using CombatServiceAPI.Characters;
using CombatServiceAPI.Passive.Models;
using System;
using System.Collections.Generic;

namespace CombatServiceAPI.Services
{
    public static class StatCalculator
    {
        public static CombatStat GetDeviantStats(CombatStat stat, Effect effect, string rarity)
        {
            CombatStat deviantStats = new CombatStat(0, 0, 0, 0, 0, 0, 0, 0);
            string amtString = Convert.ToString(effect.amount);
            float amtPerRariry = 0f;
            if (amtString.Contains("|"))
            {
                switch ((Rarity)Enum.Parse(typeof(Rarity), rarity, true))
                {
                    case Rarity.Common:
                        amtPerRariry = float.Parse(effect.amount.ToString().Split("|")[0]);
                        break;
                    case Rarity.Uncommon:
                        amtPerRariry = float.Parse(effect.amount.ToString().Split("|")[1]);
                        break;
                    case Rarity.Rare:
                        amtPerRariry = float.Parse(effect.amount.ToString().Split("|")[2]);
                        break;
                    case Rarity.Epic:
                        amtPerRariry = float.Parse(effect.amount.ToString().Split("|")[3]);
                        break;
                    case Rarity.Legendary:
                        amtPerRariry = float.Parse(effect.amount.ToString().Split("|")[4]);
                        break;
                    case Rarity.Emperor:
                        amtPerRariry = float.Parse(effect.amount.ToString().Split("|")[5]);
                        break;
                }
            }
            else
            {
                amtPerRariry = float.Parse(amtString);
            }
            if (effect.amountType == AmountType.PERCENT.ToString())
            {
                switch (effect.statEffect)
                {
                    case StatEffect.HP_TARGET:
                    case StatEffect.HP_OWNER:
                        deviantStats.hp += stat.hp * (amtPerRariry / 100f);
                        break;
                    case StatEffect.SPD_TARGET:
                    case StatEffect.SPD_OWNER:
                        deviantStats.speed += stat.speed * (amtPerRariry / 100f);
                        break;
                    case StatEffect.ATK_TARGET:
                    case StatEffect.ATK_OWNER:
                        deviantStats.atk += stat.atk * (amtPerRariry / 100f);
                        break;
                    case StatEffect.DEF_TARGET:
                    case StatEffect.DEF_OWNER:
                        deviantStats.def += stat.def * (amtPerRariry / 100f);
                        break;
                    case StatEffect.DAMAGE_TARGET:
                    case StatEffect.DAMAGE_OWNER:
                        deviantStats.atk += stat.atk * (amtPerRariry / 100f);
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
            if (effect.amountType == AmountType.PERCENT.ToString())
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

        public static CombatStat CalculateTakenHp(float totalDamage, CombatStat targetCombatStat, Effect effect, string rarity)
        {
            if (totalDamage > targetCombatStat.shieldAmt)
            {
                totalDamage -= targetCombatStat.shieldAmt;
                targetCombatStat.shieldAmt = 0;
            }
            else
            {
                targetCombatStat.shieldAmt -= totalDamage;
                totalDamage = 0;
            }
            if (targetCombatStat.takenHp + totalDamage >= targetCombatStat.hp)
            {
                targetCombatStat.takenHp = targetCombatStat.hp;
            }
            else
            {
                targetCombatStat.takenHp += totalDamage;
            }
            return targetCombatStat;
        }
    }
}
