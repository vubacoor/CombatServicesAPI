using CombatServiceAPI.Model;
using System.Collections.Generic;
using System.Linq;
using CombatServiceAPI.Characters;
using CombatServiceAPI.Formation;
using CombatServiceAPI.Passive.Interfaces;
using CombatServiceAPI.Passive.Decorators;
using CombatServiceAPI.Passive.Base;
using CombatServiceAPI.Passive.Models;
using CombatServiceAPI.Models;
using System;

namespace CombatServiceAPI.Modules
{
    public class Battle
    {
        private List<Character> userCharacters;
        private List<Character> opponentCharacters;
        private FormationController formationController;
        private Dictionary<string, Dictionary<string, Effect>> effects;

        public Battle()
        {

        }

        public Dictionary<string, Dictionary<string, Effect>> GetEffects()
        {
            return effects;
        }

        public Battle(List<Character> _userCharacters, List<Character> _opponentCharacters, Dictionary<string, Dictionary<string, Effect>> _effects)
        {
            effects = new Dictionary<string, Dictionary<string, Effect>>();
            effects = _effects;
            foreach (var item in effects)
            {
                string effectType = item.Key;
                Dictionary<string, Effect> effects = item.Value;
                foreach (var effectItem in effects)
                {
                    string effectId = effectItem.Key;
                    Effect effect = effectItem.Value;
                    effect.id = effectType + "" + effectId;
                }
            }
            formationController = new FormationController();
            userCharacters = new List<Character>();
            opponentCharacters = new List<Character>();
            for (int i = 0; i < _userCharacters.Count; i++)
            {
                Character currentChar = _userCharacters[i];
                currentChar.side = "user";
                string race = currentChar.baseStat.race;
                string element = currentChar.baseStat.element;
                Dictionary<string, List<Effect>> characterEffects = GetCharacterEffects(race, element);
                currentChar.ClassifyEffects(characterEffects);
                userCharacters.Add(currentChar);
                formationController.AddCharacter(currentChar, "user");
            }
            for (int i = 0; i < _opponentCharacters.Count; i++)
            {
                Character currentChar = _opponentCharacters[i];
                currentChar.side = "opponent";
                string race = currentChar.baseStat.race;
                string element = currentChar.baseStat.element;
                Dictionary<string, List<Effect>> characterEffects = GetCharacterEffects(race, element);
                currentChar.ClassifyEffects(characterEffects);
                opponentCharacters.Add(currentChar);
                formationController.AddCharacter(currentChar, "opponent");
            }
        }

        public Dictionary<string, List<Effect>> GetCharacterEffects(string race, string element)
        {
            Dictionary<string, List<Effect>> characterEffects = new Dictionary<string, List<Effect>>();
            characterEffects.Add("element", new List<Effect>());
            characterEffects.Add("race", new List<Effect>());
            foreach (var item in this.effects["element"])
            {
                Effect effect = item.Value;
                if (effect.name == element)
                {
                    characterEffects["element"].Add(effect);
                }
            }
            foreach (var item in this.effects["race"])
            {
                Effect effect = item.Value;
                if (effect.name == race)
                {
                    characterEffects["race"].Add(effect);
                }
            }
            return characterEffects;
        }

        public List<CombatTurn> GetCombatData()
        {
            List<CombatTurn> battleProgress = new List<CombatTurn>();
            List<CombatStat> result = new List<CombatStat>();
            List<TestPassiveData> passiveDataList = new List<TestPassiveData>();
            CombatTurn starGameCombatTurn = new CombatTurn(0, new List<EffectOutput>(), null);
            List<Character> allCharacters = new List<Character>(userCharacters.Count + opponentCharacters.Count);
            allCharacters.AddRange(userCharacters);
            allCharacters.AddRange(opponentCharacters);

            allCharacters.ForEach(character =>
            {
                character.startGameEffects.ForEach(effect =>
                {
                    List<EffectOutput> effectOutputs = BattleLogic.GetEffectOutput(effect, effect.target, userCharacters, opponentCharacters, character, character.side, 0);
                    if (effectOutputs != null && effectOutputs.Count > 0)
                    {
                        effectOutputs.ForEach(effectOutput =>
                        {
                            starGameCombatTurn.startTurnEffects.Add(effectOutput);
                        });
                    }
                });
            });

            battleProgress.Add(starGameCombatTurn);

            List<Character> orderQueue = new List<Character>(userCharacters.Count + opponentCharacters.Count);
            orderQueue.AddRange(userCharacters);
            orderQueue.AddRange(opponentCharacters);

            for (int i = 0; i < 5; i++)
            {
                CombatTurn currentCombatTurn = new CombatTurn();
                currentCombatTurn.turn = i + 1;
                currentCombatTurn.startTurnEffects = new List<EffectOutput>();
                currentCombatTurn.orders = new List<CombatOrder>();
                allCharacters.ForEach(character =>
                {
                    character.startTurnEffects.ForEach(effect =>
                    {
                        bool canTrigger = BattleLogic.CheckIfCanTriggerEffect(effect.cost, effect.rate, i + 1, effect.stackable);
                        if (canTrigger)
                        {
                            List<EffectOutput> effectOutputs = BattleLogic.GetEffectOutput(effect, effect.target, userCharacters, opponentCharacters, character, character.side, i + 1);
                            if (effectOutputs != null && effectOutputs.Count > 0)
                            {
                                effectOutputs.ForEach(effectOutput =>
                                {
                                    currentCombatTurn.startTurnEffects.Add(effectOutput);
                                });
                            };
                        }
                    });
                });

                orderQueue = orderQueue.OrderByDescending(character => character.combatStat.speed).ToList();

                int order = 1;

                orderQueue.ForEach(character =>
                {
                    currentCombatTurn.orders.Add(new CombatOrder(order, new List<EffectOutput>()));
                    character.startCharacterEffects.ForEach(effect =>
                    {
                        bool canTrigger = BattleLogic.CheckIfCanTriggerEffect(effect.cost, effect.rate, i + 1, effect.stackable);
                        if (canTrigger)
                        {
                            List<EffectOutput> effectOutputs = BattleLogic.GetEffectOutput(effect, effect.target, userCharacters, opponentCharacters, character, character.side, i + 1);
                            if (effectOutputs != null && effectOutputs.Count > 0)
                            {
                                effectOutputs.ForEach(effectOutput =>
                                {
                                    currentCombatTurn.orders[currentCombatTurn.orders.Count - 1].effectOutputs.Add(effectOutput);
                                });
                            }
                        }
                    });
                    bool canUltimate = character.CheckIfcanUseUltimate(i + 1);
                    if (canUltimate)
                    {
                        character.ultimateEffects.ForEach(effect =>
                        {
                            List<EffectOutput> effectOutputs = BattleLogic.GetEffectOutput(effect, effect.target, userCharacters, opponentCharacters, character, character.side, i + 1);
                            if (effectOutputs != null && effectOutputs.Count > 0)
                            {
                                effectOutputs.ForEach(effectOutput =>
                                {
                                    currentCombatTurn.orders[currentCombatTurn.orders.Count - 1].effectOutputs.Add(effectOutput);
                                });
                            }
                        });
                    }
                    else
                    {
                        character.normalSkillEffects.ForEach(effect =>
                        {
                            List<EffectOutput> effectOutputs = BattleLogic.GetEffectOutput(effect, effect.target, userCharacters, opponentCharacters, character, character.side, i + 1);
                            string normalSkillTargetType = effect.target;
                            if (effectOutputs != null && effectOutputs.Count > 0)
                            {
                                effectOutputs.ForEach(effectOutput =>
                                {
                                    currentCombatTurn.orders[currentCombatTurn.orders.Count - 1].effectOutputs.Add(effectOutput);
                                });
                            }
                            List<Effect> overrideEffectsBaseOnType = character.GetOverrideEffectsBaseOnType(effect);
                            overrideEffectsBaseOnType.ForEach(overrideEffect =>
                            {
                                string targetType = overrideEffect.target;
                                if (targetType == "OVERRIDE_TARGET")
                                {
                                    targetType = normalSkillTargetType;
                                }
                                List<EffectOutput> effectOutputs = BattleLogic.GetEffectOutput(overrideEffect, targetType, userCharacters, opponentCharacters, character, character.side, i + 1);

                                if (effectOutputs != null && effectOutputs.Count > 0)
                                {
                                    effectOutputs.ForEach(effectOutput =>
                                    {
                                        currentCombatTurn.orders[currentCombatTurn.orders.Count - 1].effectOutputs.Add(effectOutput);
                                    });
                                }
                            });
                        });
                    }
                    Dictionary<string, CombatStat> characterStats = new Dictionary<string, CombatStat>();
                    allCharacters.ForEach(character =>
                    {
                        CombatStat combatStat = new CombatStat(
                            character.combatStat.atk,
                            character.combatStat.def,
                            character.combatStat.speed,
                            character.combatStat.hp,
                            character.combatStat.takenHp,
                            character.combatStat.reduceDamage,
                            character.combatStat.crit,
                            character.combatStat.luck);
                        characterStats.Add(character._id, combatStat);
                    });
                    currentCombatTurn.orders[currentCombatTurn.orders.Count - 1].characterStats = characterStats;
                    order++;
                });
                battleProgress.Add(currentCombatTurn);
            }
            return battleProgress;
        }
    }

    //public int GetResult()
    //{
    //    int result = 0;
    //    if (opponentCharacters.Where(character => character.hp > 0).Count() == 0 && userCharacters.Where(character => character.hp > 0).Count() > 0)
    //    {
    //        result = 1;
    //    }
    //    else if (userCharacters.Where(character => character.hp > 0).Count() == 0 && opponentCharacters.Where(character => character.hp > 0).Count() > 0)
    //    {
    //        result = -1;
    //    }
    //    else
    //    {
    //        result = 0;
    //    }
    //    return result;
    //}

    //public bool CheckIfFinishedCombat()
    //{
    //    bool isFinished = false;
    //    if (userCharacters.Where(character => character.hp > 0).Count() == 0 || opponentCharacters.Where(character => character.hp > 0).Count() == 0)
    //    {
    //        isFinished = true;
    //    }
    //    else
    //    {
    //        isFinished = false;
    //    }
    //    return isFinished;
    //}
}

