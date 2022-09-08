using System;
using System.Collections.Generic;
using System.Text.Json;
using CombatServiceAPI.Characters;
using CombatServiceAPI.Models;
using CombatServiceAPI.Modules;
using CombatServiceAPI.Passive.Models;
using CombatServiceAPI.Services;

namespace CombatServiceAPI.Controllers
{
    public class EffectController
    {
        public Dictionary<string, Dictionary<string, Effect>> effects;
        public Dictionary<string, RemainEffect> remainingEffects;
        public List<Effect> startGameEffects;
        public List<Effect> startTurnEffects;
        public List<Effect> startOrderEffects;
        public List<Effect> endOrderEffects;
        public List<Effect> startCharacterEffects;
        public List<Effect> overrideEffects;
        public List<Effect> endCharacterEffects;
        public List<Effect> normalSkillEffects;
        public List<Effect> ultimateEffects;

        public EffectController()
        {
            startGameEffects = new List<Effect>();
            startCharacterEffects = new List<Effect>();
            startTurnEffects = new List<Effect>();
            endCharacterEffects = new List<Effect>();
            overrideEffects = new List<Effect>();
            normalSkillEffects = new List<Effect>();
            ultimateEffects = new List<Effect>();
            endOrderEffects = new List<Effect>();
            startOrderEffects = new List<Effect>();
            remainingEffects = new Dictionary<string, RemainEffect>();
        }

        public void ClassifyEffects(Dictionary<string, List<Effect>> _characterEffects)
        {
            foreach (var item in _characterEffects)
            {
                var effects = item.Value;
                effects.ForEach(effect =>
                {
                    switch (effect.phaseTrigger)
                    {
                        case PhaseTrigger.START_GAME:
                            startGameEffects.Add(effect);
                            break;
                        case PhaseTrigger.START_TURN:
                            startTurnEffects.Add(effect);
                            break;
                        case PhaseTrigger.START_CHARACTER:
                            startCharacterEffects.Add(effect);
                            break;
                        case PhaseTrigger.END_CHARACTER:
                            endCharacterEffects.Add(effect);
                            break;
                        case PhaseTrigger.ACTION:
                            if (effect.type == EffectType.Active)
                            {
                                normalSkillEffects.Add(effect);
                            }
                            else
                            {
                                ultimateEffects.Add(effect);
                            }
                            break;
                        case PhaseTrigger.OVERRIDE_PHASE:
                            overrideEffects.Add(effect);
                            break;
                        case PhaseTrigger.START_ORDER:
                            startOrderEffects.Add(effect);
                            break;
                        case PhaseTrigger.END_ORDER:
                            endOrderEffects.Add(effect);
                            break;
                    }
                });
            }
        }

        public void AddEffect(Effect effect)
        {

        }
        public void RemoveEffect(Effect effect)
        {

        }
        public void ApplyEffect(Character target, Effect effect, CombatStat baseStatLite, CombatStat combatStat, string rarity, int currentTurn)
        {
            CombatStat updatedStat = new CombatStat(
                 combatStat.atk,
                 combatStat.def,
                 combatStat.speed,
                 combatStat.hp,
                 combatStat.takenHp,
                 combatStat.shieldAmt,
                 combatStat.crit,
                 combatStat.luck);
            if (BattleLogic.CheckIfCanTriggerEffect(effect, combatStat, currentTurn))
            {
                CombatStat bonusStat = StatCalculator.GetDeviantStats(baseStatLite, effect, rarity);
                updatedStat.atk += bonusStat.atk;
                updatedStat.hp += bonusStat.hp;
                updatedStat.speed += bonusStat.speed;
                updatedStat.def += bonusStat.def;
                updatedStat.shieldAmt += bonusStat.shieldAmt;
                if (effect.expireTurn > 1)
                {
                    CombatStat tempStat = new CombatStat(updatedStat.atk,
                     updatedStat.def,
                     updatedStat.speed,
                     updatedStat.hp,
                     updatedStat.takenHp,
                     updatedStat.shieldAmt,
                     updatedStat.crit,
                     updatedStat.luck);
                    RemainEffect remainEffect = new RemainEffect();
                    remainEffect.effect = effect;
                    remainEffect.tempStat = tempStat;
                    remainEffect.expireFor = effect.expireTurn - 1;
                    remainingEffects.Add(effect.id, remainEffect);
                }
            }
            target.combatStat = updatedStat;
        }
        public void ApplyActiveEffect(Effect effect, CombatStat combatStat, string rarity, Character target, int currentTurn)
        {
            if (BattleLogic.CheckIfCanTriggerEffect(effect, combatStat, currentTurn))
            {
                if (effect.effectBase == EffectBase.ELEMENT_DAMAGE.ToString() || effect.effectBase == EffectBase.FLAT_DAMAGE.ToString())
                {
                    CombatStat targetStat = new CombatStat(
                        target.combatStat.atk,
                        target.combatStat.def,
                        target.combatStat.speed,
                        target.combatStat.hp,
                        target.combatStat.takenHp,
                        target.combatStat.shieldAmt,
                        target.combatStat.crit,
                        target.combatStat.luck
                        );
                    CombatStat bonusStat = StatCalculator.GetDeviantStats(combatStat, effect, rarity);
                    CombatStat updatedTargetStat = StatCalculator.CalculateTakenHp(bonusStat.atk, targetStat, effect, target.rarity);
                    target.combatStat = updatedTargetStat;
                }
            }
        }
        public void ApplyRecoverEffect(Effect effect, Character target, int currentTurn)
        {
            float recoverAmt;
            if (BattleLogic.CheckIfCanTriggerEffect(effect, target.combatStat, currentTurn))
            {
                if (target.combatStat.takenHp > 0)
                {
                    recoverAmt = StatCalculator.GetRecoverAmt(target.combatStat, effect);
                    if (target.combatStat.takenHp - recoverAmt >= 0)
                    {
                        target.combatStat.takenHp -= recoverAmt;
                    }
                    else
                    {
                        target.combatStat.takenHp = 0;
                    }
                }
            }
        }
        public void ApplyShieldEffect(Effect effect, Character target, int currentTurn)
        {
            CombatStat cloneCombatStat = new CombatStat(
                target.combatStat.atk,
                target.combatStat.def,
                target.combatStat.speed,
                target.combatStat.hp,
                target.combatStat.takenHp,
                target.combatStat.shieldAmt,
                target.combatStat.crit,
                target.combatStat.luck
                );
            target.combatStat.shieldAmt += StatCalculator.GetDeviantStats(cloneCombatStat, effect, target.rarity).def;
        }
        public void ApplySelfExplosionEffect(Effect effect, CombatStat combatStat, Character target, string rarity)
        {
            float totalDamage = StatCalculator.GetDeviantStats(combatStat, effect, rarity).hp;
            combatStat.takenHp = combatStat.hp;
            CombatStat targetStat = new CombatStat(
                        target.combatStat.atk,
                        target.combatStat.def,
                        target.combatStat.speed,
                        target.combatStat.hp,
                        target.combatStat.takenHp,
                        target.combatStat.shieldAmt,
                        target.combatStat.crit,
                        target.combatStat.luck
                        );
            CombatStat updatedTargetStat = StatCalculator.CalculateTakenHp(totalDamage, targetStat, effect, target.rarity);
            target.combatStat = updatedTargetStat;
        }
        /// <summary>
        /// Get effect output
        /// </summary>
        /// <param name="effect">
        /// Effect need to get effect output
        /// </param>
        /// <param name="targetType">
        /// effect's target type
        /// </param>
        /// <param name="userCharacters">
        /// list of user characters
        /// </param>
        /// <param name="opponentCharacters">
        /// list of opponnent characters
        /// </param>
        /// <param name="caster">
        /// Caster of effect
        /// </param>
        /// <param name="casterSide">
        /// side of caster (user or opponent)
        /// </param>
        /// <param name="currentTurn">
        /// current turn
        /// </param>
        /// <returns></returns>
        public List<EffectOutput> ProcessEffect(Effect effect, string targetType, List<Character> userCharacters, List<Character> opponentCharacters, Character caster, string casterSide, int currentTurn)
        {
            List<EffectOutput> effectOutputs = new List<EffectOutput>();
            List<Character> targets = BattleLogic.GetTargets(targetType, userCharacters, opponentCharacters, caster, casterSide);

            if (targets != null && targets.Count > 0)
            {
                if (targets[0] != null)
                {
                    if (effect.additionalEffect == "NULL" || effect.additionalEffect == AdditionalEffect.RECOVER_COMBINE.ToString())
                    {
                        if (effect.effectBase == EffectBase.STAT_CHANGE.ToString())
                        {
                            targets.ForEach(target =>
                            {
                                target.ApplyEffect(target, effect, currentTurn);
                                CombatStat targetNewStat = new CombatStat(target.combatStat.atk, target.combatStat.def, target.combatStat.speed, target.combatStat.hp, target.combatStat.takenHp, target.combatStat.shieldAmt, target.combatStat.crit, target.combatStat.luck);
                                if (effect.expireTurn == -1)
                                {
                                    effectOutputs.Add(new EffectOutput(target._id, target._id, effect.effectBase, effect.statEffect, effect.additionalEffect, targetNewStat));
                                }
                                else
                                {
                                    effectOutputs.Add(new EffectOutput(target._id, target._id, effect.effectBase, effect.statEffect, effect.additionalEffect, effect.expireTurn, targetNewStat));
                                }
                            });
                        }
                        else if (effect.effectBase == EffectBase.ELEMENT_DAMAGE.ToString() || effect.effectBase == EffectBase.FLAT_DAMAGE.ToString())
                        {
                            targets.ForEach(target =>
                            {
                                caster.ApplyActiveEffect(effect, target, currentTurn);
                                CombatStat targetNewStat = new CombatStat(target.combatStat.atk, target.combatStat.def, target.combatStat.speed, target.combatStat.hp, target.combatStat.takenHp, target.combatStat.shieldAmt, target.combatStat.crit, target.combatStat.luck);
                                effectOutputs.Add(new EffectOutput(caster._id, target._id, effect.effectBase, effect.statEffect, effect.additionalEffect, targetNewStat));
                            });
                        }
                        else if (effect.effectBase == EffectBase.ELEMENT_RECOVER.ToString())
                        {
                            targets.ForEach(target =>
                            {
                                caster.ApplyRecoverEffect(effect, target, currentTurn);
                                CombatStat targetNewStat = new CombatStat(target.combatStat.atk, target.combatStat.def, target.combatStat.speed, target.combatStat.hp, target.combatStat.takenHp, target.combatStat.shieldAmt, target.combatStat.crit, target.combatStat.luck);
                                effectOutputs.Add(new EffectOutput(caster._id, target._id, effect.effectBase, effect.statEffect, effect.additionalEffect, targetNewStat));
                            });
                        }
                        else if (effect.effectBase == EffectBase.FLAT_SHIELD.ToString())
                        {
                            targets.ForEach(target =>
                            {
                                caster.ApplyShieldEffect(effect, target, currentTurn);
                                CombatStat targetNewStat = new CombatStat(target.combatStat.atk, target.combatStat.def, target.combatStat.speed, target.combatStat.hp, target.combatStat.takenHp, target.combatStat.shieldAmt, target.combatStat.crit, target.combatStat.luck);
                                effectOutputs.Add(new EffectOutput(caster._id, target._id, effect.effectBase, effect.statEffect, effect.additionalEffect, targetNewStat));
                            });
                        }
                    }
                    else
                    {
                        if (effect.effectBase == EffectBase.ELEMENT_DAMAGE.ToString() || effect.effectBase == EffectBase.FLAT_DAMAGE.ToString())
                        {
                            if (effect.additionalEffect == AdditionalEffect.CHAIN_ATTACK.ToString())
                            {
                                string chainAmtString = JsonSerializer.Serialize(effect.additionalData);
                                int chainAmt = Int32.Parse(chainAmtString);
                                for (int i = 0; i < chainAmt; i++)
                                {
                                    targets.ForEach(target =>
                                    {
                                        caster.ApplyActiveEffect(effect, target, currentTurn);
                                        CombatStat targetNewStat = new CombatStat(target.combatStat.atk, target.combatStat.def, target.combatStat.speed, target.combatStat.hp, target.combatStat.takenHp, target.combatStat.shieldAmt, target.combatStat.crit, target.combatStat.luck);
                                        effectOutputs.Add(new EffectOutput(caster._id, target._id, effect.effectBase, effect.statEffect, effect.additionalEffect, targetNewStat));
                                    });
                                }
                            }
                        }
                        else if (effect.effectBase == EffectBase.FLAT_RECOVER.ToString() || effect.effectBase == EffectBase.ELEMENT_RECOVER.ToString())
                        {
                            targets.ForEach(target =>
                            {
                                caster.ApplyRecoverEffect(effect, target, currentTurn);
                                CombatStat targetNewStat = new CombatStat(target.combatStat.atk, target.combatStat.def, target.combatStat.speed, target.combatStat.hp, target.combatStat.takenHp, target.combatStat.shieldAmt, target.combatStat.crit, target.combatStat.luck);
                                effectOutputs.Add(new EffectOutput(caster._id, target._id, effect.effectBase, effect.statEffect, effect.additionalEffect, targetNewStat));
                            });
                        }
                        else
                        {
                            targets.ForEach(target =>
                            {
                                caster.ApplySpecialEffect(effect, target);
                                CombatStat targetNewStat = new CombatStat(target.combatStat.atk, target.combatStat.def, target.combatStat.speed, target.combatStat.hp, target.combatStat.takenHp, target.combatStat.shieldAmt, target.combatStat.crit, target.combatStat.luck);
                                effectOutputs.Add(new EffectOutput(caster._id, target._id, effect.effectBase, effect.statEffect, effect.additionalEffect, targetNewStat));
                            });
                        }
                    }
                }
            }
            return effectOutputs;
        }
        public void ApplyFireRoarEffect(Character target, Effect effect)
        {
            CombatStat baseStatLite = new CombatStat(
                target.baseStat.atk,
                target.baseStat.def,
                target.baseStat.speed,
                target.baseStat.hp,
                0,
                0,
                target.baseStat.crit,
                target.baseStat.luck);
            target.combatStat.atk += StatCalculator.GetDeviantStats(baseStatLite, effect, target.rarity).atk;
        }
    }
}
