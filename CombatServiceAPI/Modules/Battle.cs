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

        /// <summary>
        /// Init battle
        /// </summary>
        /// <param name="_userCharacters">
        /// list characters of user
        /// </param>
        /// <param name="_opponentCharacters">
        /// list character of opponent
        /// </param>
        /// <param name="_effects">
        /// list of all effects from config json
        /// </param>
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
        /// <summary>
        /// Get character effects base on race & element
        /// </summary>
        /// <param name="race">
        /// character's race
        /// </param>
        /// <param name="element">
        /// character's element
        /// </param>
        /// <returns>
        /// character effects dictionary with specific by type element and race
        /// </returns>
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
        /// <summary>
        /// Get combat data
        /// </summary>
        /// <returns>
        /// Combat data
        /// </returns>
        public BattleData GetCombatData()
        {
            BattleData batleData = new BattleData();
            List<CombatTurn> battleProgress = new List<CombatTurn>();
            List<TestPassiveData> passiveDataList = new List<TestPassiveData>();
            StartTurn startTurn = new StartTurn();
            startTurn.effects = new List<EffectOutput>();
            startTurn.characterStats = new Dictionary<string, CombatStat>();
            CombatTurn startGameCombatTurn = new CombatTurn(0, startTurn, null);
            List<Character> allCharacters = new List<Character>(userCharacters.Count + opponentCharacters.Count);
            allCharacters.AddRange(userCharacters);
            allCharacters.AddRange(opponentCharacters);

            List<EffectOutput> startGameEffectOutputs = GetStartGameEffectOutputs(allCharacters);

            startGameCombatTurn.startTurn.effects = startGameEffectOutputs;
            Dictionary<string, CombatStat> startGameUpadatedCombatStats = GetUpdatedCombatStats();
            startGameCombatTurn.startTurn.characterStats = startGameUpadatedCombatStats;

            battleProgress.Add(startGameCombatTurn);

            List<Character> orderQueue = new List<Character>(userCharacters.Count + opponentCharacters.Count);
            orderQueue.AddRange(userCharacters);
            orderQueue.AddRange(opponentCharacters);
            int turn = 0;
            do
            {
                CombatTurn currentCombatTurn = new CombatTurn();
                turn++;

                currentCombatTurn.turn = turn;
                currentCombatTurn.startTurn = new StartTurn();
                currentCombatTurn.startTurn.effects = new List<EffectOutput>();
                currentCombatTurn.startTurn.characterStats = new Dictionary<string, CombatStat>();
                currentCombatTurn.orders = new List<CombatOrder>();
                allCharacters = allCharacters.Where(character => !BattleLogic.CheckIfDead(character)).ToList();
                List<EffectOutput> startTurnEffectOutputs = GetStartTurnEffectOutputs(allCharacters, turn);
                currentCombatTurn.startTurn.effects = startTurnEffectOutputs;
                Dictionary<string, CombatStat> startTurnUpdatedCombatStats = GetUpdatedCombatStats();
                currentCombatTurn.startTurn.characterStats = startTurnUpdatedCombatStats;

                orderQueue = orderQueue.Where(character => !BattleLogic.CheckIfDead(character)).OrderByDescending(character => character.combatStat.speed).ToList();

                int order = 1;

                for (int i = 0; i < orderQueue.Count; i++)
                {
                    if (!BattleLogic.CheckIfFinishedCombat(userCharacters, opponentCharacters))
                    {
                        Character character = orderQueue[i];
                        if (!BattleLogic.CheckIfDead(character))
                        {
                            currentCombatTurn.orders.Add(new CombatOrder(order, character._id, new List<EffectOutput>(), new List<EffectOutput>()));
                            List<EffectOutput> startCharacterEffectOutputs = GetStartCharacterEffectOutputs(character, turn);
                            currentCombatTurn.orders[currentCombatTurn.orders.Count - 1].actionEffects = startCharacterEffectOutputs;
                            bool canUltimate = character.CheckIfcanUseUltimate(turn);
                            if (canUltimate)
                            {
                                List<EffectOutput> ultimateEffectOutputs = GetCharacterUltimateEffectOutputs(character, turn);
                                currentCombatTurn.orders[currentCombatTurn.orders.Count - 1].actionEffects = ultimateEffectOutputs;
                            }
                            else
                            {
                                List<EffectOutput> normalSkillEffectOutputs = GetNormalSkillEffectOutputs(character, turn);
                                currentCombatTurn.orders[currentCombatTurn.orders.Count - 1].actionEffects = normalSkillEffectOutputs;
                            }
                            List<Character> filteredCharacters = allCharacters.Where(character => !BattleLogic.CheckIfDead(character)).ToList();
                            List<EffectOutput> endOrderEffectOutputs = GetEndOrderEffectOutputs(filteredCharacters, turn);
                            currentCombatTurn.orders[currentCombatTurn.orders.Count - 1].endOrderEffects = endOrderEffectOutputs;
                            Dictionary<string, CombatStat> endOrderUpdatedCombatStats = GetUpdatedCombatStats();
                            currentCombatTurn.orders[currentCombatTurn.orders.Count - 1].characterStats = endOrderUpdatedCombatStats;
                            order++;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                battleProgress.Add(currentCombatTurn);
            } while (!BattleLogic.CheckIfFinishedCombat(userCharacters, opponentCharacters));
            batleData.status = BattleLogic.GetResult(userCharacters, opponentCharacters);
            batleData.battleProgress = battleProgress;
            return batleData;
        }
        public List<EffectOutput> GetStartGameEffectOutputs(List<Character> allCharacters)
        {
            List<EffectOutput> startGameEffectOutputs = new List<EffectOutput>();
            allCharacters.ForEach(character =>
            {
                character.effectController.startGameEffects.ForEach(effect =>
                {
                    List<EffectOutput> effectOutputs = character.effectController.ProcessEffect(effect, effect.target, userCharacters, opponentCharacters, character, character.side, 0);
                    if (effectOutputs != null && effectOutputs.Count > 0)
                    {
                        effectOutputs.ForEach(effectOutput =>
                        {
                            startGameEffectOutputs.Add(effectOutput);
                        });
                    }
                });
            });
            return startGameEffectOutputs;
        }
        public List<EffectOutput> GetStartTurnEffectOutputs(List<Character> allCharacters, int turn)
        {
            List<EffectOutput> startTurnEffectOutputs = new List<EffectOutput>();
            allCharacters.ForEach(character =>
            {
                character.effectController.startTurnEffects.ForEach(effect =>
                {
                    bool canTrigger = BattleLogic.CheckIfCanTriggerEffect(effect, character.combatStat, turn);
                    if (canTrigger)
                    {
                        List<EffectOutput> effectOutputs = character.effectController.ProcessEffect(effect, effect.target, userCharacters, opponentCharacters, character, character.side, turn);
                        if (effectOutputs != null && effectOutputs.Count > 0)
                        {
                            effectOutputs.ForEach(effectOutput =>
                            {
                                startTurnEffectOutputs.Add(effectOutput);
                            });
                        };
                    }
                });
            });
            return startTurnEffectOutputs;
        }
        public List<EffectOutput> GetStartCharacterEffectOutputs(Character character, int turn)
        {
            List<EffectOutput> startCharacterEffectOutputs = new List<EffectOutput>();
            character.effectController.startCharacterEffects.ForEach(effect =>
            {
                bool canTrigger = BattleLogic.CheckIfCanTriggerEffect(effect, character.combatStat, turn);
                if (canTrigger)
                {
                    List<EffectOutput> effectOutputs = character.effectController.ProcessEffect(effect, effect.target, userCharacters, opponentCharacters, character, character.side, turn);
                    if (effectOutputs != null && effectOutputs.Count > 0)
                    {
                        effectOutputs.ForEach(effectOutput =>
                        {
                            startCharacterEffectOutputs.Add(effectOutput);
                        });
                    }
                }
            });
            return startCharacterEffectOutputs;
        }
        public List<EffectOutput> GetCharacterUltimateEffectOutputs(Character character, int turn)
        {
            List<EffectOutput> ultimateEffectOutputs = new List<EffectOutput>();
            character.effectController.ultimateEffects.ForEach(effect =>
            {
                List<EffectOutput> effectOutputs = character.effectController.ProcessEffect(effect, effect.target, userCharacters, opponentCharacters, character, character.side, turn);
                if (effectOutputs != null && effectOutputs.Count > 0)
                {
                    effectOutputs.ForEach(effectOutput =>
                    {
                        ultimateEffectOutputs.Add(effectOutput);
                    });
                }
                List<Effect> overrideEffectsBaseOnType = character.GetOverrideEffectsBaseOnType(effect);
                overrideEffectsBaseOnType.ForEach(overrideEffect =>
                {
                    string targetType = overrideEffect.target;
                    string ultimateSkillTargetType = effect.target;
                    if (targetType == Target.OVERRIDE_TARGET.ToString())
                    {
                        targetType = ultimateSkillTargetType;
                    }
                    List<EffectOutput> effectOutputs = character.effectController.ProcessEffect(overrideEffect, targetType, userCharacters, opponentCharacters, character, character.side, turn);

                    if (effectOutputs != null && effectOutputs.Count > 0)
                    {
                        effectOutputs.ForEach(effectOutput =>
                        {
                            ultimateEffectOutputs.Add(effectOutput);
                        });
                    }
                });
            });
            return ultimateEffectOutputs;
        }
        public List<EffectOutput> GetNormalSkillEffectOutputs(Character character, int turn)
        {
            List<EffectOutput> normalSkillEffectOutputs = new List<EffectOutput>();
            character.effectController.normalSkillEffects.ForEach(effect =>
            {
                List<EffectOutput> effectOutputs = character.effectController.ProcessEffect(effect, effect.target, userCharacters, opponentCharacters, character, character.side, turn);
                string normalSkillTargetType = effect.target;
                if (effectOutputs != null && effectOutputs.Count > 0)
                {
                    effectOutputs.ForEach(effectOutput =>
                    {
                        normalSkillEffectOutputs.Add(effectOutput);
                    });
                }
                List<Effect> overrideEffectsBaseOnType = character.GetOverrideEffectsBaseOnType(effect);
                overrideEffectsBaseOnType.ForEach(overrideEffect =>
                {
                    string targetType = overrideEffect.target;
                    if (targetType == Target.OVERRIDE_TARGET.ToString())
                    {
                        targetType = normalSkillTargetType;
                    }
                    List<EffectOutput> effectOutputs = character.effectController.ProcessEffect(overrideEffect, targetType, userCharacters, opponentCharacters, character, character.side, turn);

                    if (effectOutputs != null && effectOutputs.Count > 0)
                    {
                        effectOutputs.ForEach(effectOutput =>
                        {
                            normalSkillEffectOutputs.Add(effectOutput);
                        });
                    }
                });
            });
            return normalSkillEffectOutputs;
        }
        public List<EffectOutput> GetEndOrderEffectOutputs(List<Character> filteredCharacters, int turn)
        {
            List<EffectOutput> endOrderEffectOutputs = new List<EffectOutput>();
            filteredCharacters.Where(character => !BattleLogic.CheckIfDead(character)).ToList().ForEach(filteredCharacter =>
            {
                filteredCharacter.effectController.endOrderEffects.ForEach(effect =>
                {
                    bool canTrigger = BattleLogic.CheckIfCanTriggerEffect(effect, filteredCharacter.combatStat, turn);
                    if (canTrigger)
                    {
                        List<EffectOutput> effectOutputs = filteredCharacter.effectController.ProcessEffect(effect, effect.target, userCharacters, opponentCharacters, filteredCharacter, filteredCharacter.side, turn);
                        if (effectOutputs != null && effectOutputs.Count > 0)
                        {
                            effectOutputs.ForEach(effectOutput =>
                            {
                                endOrderEffectOutputs.Add(effectOutput);
                            });
                        }
                    }
                });
            });
            return endOrderEffectOutputs;
        }
        public Dictionary<string, CombatStat> GetUpdatedCombatStats()
        {
            Dictionary<string, CombatStat> combatStats = new Dictionary<string, CombatStat>();

            List<Character> allCharacters = new List<Character>(userCharacters.Count + opponentCharacters.Count);
            allCharacters.AddRange(userCharacters);
            allCharacters.AddRange(opponentCharacters);
            allCharacters.ForEach(character =>
            {
                CombatStat combatStat = new CombatStat(
                character.combatStat.atk,
                character.combatStat.def,
                character.combatStat.speed,
                character.combatStat.hp,
                character.combatStat.takenHp,
                character.combatStat.shieldAmt,
                character.combatStat.crit,
                character.combatStat.luck);
                combatStats.Add(character._id, combatStat);
            });
            return combatStats;
        }
    }
}

