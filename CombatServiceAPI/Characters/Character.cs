using System;
using System.Collections.Generic;
using CombatServiceAPI.Passive.Interfaces;
using CombatServiceAPI.Passive.Models;
using CombatServiceAPI.Passive.Decorators;
using CombatServiceAPI.Passive.Base;
using CombatServiceAPI.Models;
using CombatServiceAPI.Services;
using CombatServiceAPI.Modules;
using System.Linq;

namespace CombatServiceAPI.Characters
{
    public class Character : BaseStat, ICharacterActions
    {
        public string _id { get; set; }
        public string key { get; set; }
        public string baseKey { get; set; }
        public string status { get; set; }
        public string[] itemList { get; set; }
        public string contractAddress { get; set; }
        public string nftId { get; set; }
        public int position { get; set; }
        public string side { get; set; }
        public BaseStat baseStat { get; set; }
        public CombatStat combatStat = new CombatStat(0, 0, 0, 0, 0, 0, 0, 0);
        public CombatStat tempStat = new CombatStat(0, 0, 0, 0, 0, 0, 0, 0);

        public CombatStat additionalStat;

        public List<Effect> startGameEffects;
        public List<Effect> startTurnEffects;
        public List<Effect> startOrderEffects;
        public List<Effect> endOrderEffects;
        public List<Effect> startCharacterEffects;
        public List<Effect> overrideEffects;
        public List<Effect> endCharacterEffects;
        public List<Effect> normalSkillEffects;
        public List<Effect> ultimateEffects;

        public Dictionary<string, Effect> tempEffects;

        public Character()
        {
        }

        public Character(string _id, int position, string side)
        {
            this._id = _id;
            this.position = position;
            this.side = side;
        }

        public Character(string _id, string key, int position, BaseStat baseStat)
        {
            this._id = _id;
            this.key = key;
            this.position = position;
            this.baseStat = baseStat;
        }

        public void ClassifyEffects(Dictionary<string, List<Effect>> _characterEffects)
        {
            combatStat = new CombatStat(baseStat.atk, baseStat.def, baseStat.speed, baseStat.hp, 0, 0, baseStat.crit, baseStat.luck);
            startGameEffects = new List<Effect>();
            startCharacterEffects = new List<Effect>();
            startTurnEffects = new List<Effect>();
            endCharacterEffects = new List<Effect>();
            overrideEffects = new List<Effect>();
            normalSkillEffects = new List<Effect>();
            ultimateEffects = new List<Effect>();
            endOrderEffects = new List<Effect>();
            startOrderEffects = new List<Effect>();
            tempEffects = new Dictionary<string, Effect>();
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

        public void AddTempEffect(Effect effect)
        {
            tempEffects.Add(effect.id, effect);
        }


        public void Attack()
        {
            throw new NotImplementedException();
        }

        public void ApplyEffect(Effect effect, int currentTurn)
        {
            if (BattleLogic.CheckIfCanTriggerEffect(effect.cost, effect.rate, currentTurn, effect.stackable))
            {
                if (effect.expireTurn != -1)
                {
                    CombatStat baseStatLite = new CombatStat(baseStat.atk, baseStat.def, baseStat.speed, baseStat.hp, 0, 0, baseStat.crit, baseStat.luck);
                    CombatStat bonusStat = StatCalculator.GetDeviantStats(baseStatLite, effect);
                    this.tempStat.atk += bonusStat.atk;
                    this.tempStat.hp += bonusStat.hp;
                    this.tempStat.speed += bonusStat.speed;
                    this.tempStat.def += bonusStat.def;
                    this.tempStat.reduceDamage += bonusStat.reduceDamage;
                }
                else
                {
                    CombatStat baseStatLite = new CombatStat(baseStat.atk, baseStat.def, baseStat.speed, baseStat.hp, 0, 0, baseStat.crit, baseStat.luck);
                    CombatStat bonusStat = StatCalculator.GetDeviantStats(baseStatLite, effect);
                    this.combatStat.atk += bonusStat.atk;
                    this.combatStat.hp += bonusStat.hp;
                    this.combatStat.speed += bonusStat.speed;
                    this.combatStat.def += bonusStat.def;
                    this.combatStat.reduceDamage += bonusStat.reduceDamage;
                }
            }
        }

        public void ApplyActiveEffect(Effect effect, Character target, int currentTurn)
        {
            if (BattleLogic.CheckIfCanTriggerEffect(effect.cost, effect.rate, currentTurn, effect.stackable))
            {
                if (effect.effectBase == "ELEMENT_DAMAGE" || effect.effectBase == "FLAT_DAMAGE")
                {
                    CombatStat bonusStat = StatCalculator.GetDeviantStats(this.combatStat, effect);
                    target.combatStat.takenHp += bonusStat.atk;
                }
            }
        }

        public void ApplyRecoverEffect(Effect effect, Character target, int currentTurn)
        {
            if (BattleLogic.CheckIfCanTriggerEffect(effect.cost, effect.rate, currentTurn, effect.stackable))
            {
                if (target.combatStat.takenHp > 0)
                {
                    float recoverAmt = StatCalculator.GetRecoverAmt(target.combatStat, effect);
                    if (target.combatStat.takenHp - recoverAmt >= 0)
                    {
                        target.combatStat.takenHp = target.combatStat.takenHp - recoverAmt;
                    }
                    else
                    {
                        target.combatStat.takenHp = 0;
                    }
                }
            }
        }

        public bool CheckIfcanUseUltimate(int currentTurn)
        {
            return (BattleLogic.CheckIfCanTriggerEffect(ultimateEffects[0].cost, ultimateEffects[0].rate, currentTurn, ultimateEffects[0].stackable));
        }

        public List<Effect> GetOverrideEffectsBaseOnType(Effect prevEffect)
        {
            List<Effect> overrideEffectsBaseOnType = new List<Effect>();
            switch (prevEffect.effectBase)
            {
                case "ELEMENT_DAMAGE":
                    overrideEffectsBaseOnType = overrideEffects.Where(effect => effect.type == "Active").ToList();
                    break;
                case "ELEMENT_RECOVER":
                    overrideEffectsBaseOnType = overrideEffects.Where(effect => effect.effectBase == "Buff").ToList();
                    break;
            }
            return overrideEffectsBaseOnType;
        }
    }
}
