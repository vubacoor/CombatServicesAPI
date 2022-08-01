using CombatServiceAPI.Model;
using System.Collections.Generic;
using System.Linq;
using CombatServiceAPI.Constants;
using CombatServiceAPI.Characters;
using CombatServiceAPI.Formation;

namespace CombatServiceAPI.Modules
{
    public class Battle
    {
        private List<BaseCharacter> userCharacters;
        private List<BaseCharacter> opponentCharacters;
        private FormationController formationController;

        public Battle()
        {

        }

        public Battle(List<BaseCharacter> _userCharacters, List<BaseCharacter> _opponentCharacters)
        {
            formationController = new FormationController();
            userCharacters = new List<BaseCharacter>();
            opponentCharacters = new List<BaseCharacter>();
            for (int i = 0; i < _userCharacters.Count; i++)
            {
                BaseCharacter currentChar = _userCharacters[i];
                formationController.AddCharacter(currentChar, "user");
            }
            for (int i = 0; i < _opponentCharacters.Count; i++)
            {
                BaseCharacter currentChar = _opponentCharacters[i];
                formationController.AddCharacter(currentChar, "opponent");
            }
        }

        public BattleData GetBattleData()
        {
            BattleData battleData = new BattleData();
            battleData.battleProgress = new List<BattleProgess>();
            battleData.skip = false;
            int turn = 0;
            do
            {
                turn++;
                List<BaseCharacter> orderQueue = new List<BaseCharacter>(formationController.userCharacters.Count + formationController.opponentCharacters.Count);
                orderQueue.AddRange(userCharacters);
                orderQueue.AddRange(opponentCharacters);
                orderQueue = orderQueue.OrderByDescending(character => character.speed).ToList();
                for (int i = 0; i < orderQueue.Count; i++)
                {
                    BaseCharacter currentCharacter = orderQueue[i];
                    if (CheckIfFinishedCombat())
                    {
                        break;
                    }
                    else
                    {
                        if (currentCharacter.hp > 0)
                        {
                            int order = i + 1;
                            BattleProgess battleProgress = ProgessBattleSingleOpponentTarget(currentCharacter, turn, order);
                            battleData.battleProgress.Add(battleProgress);
                        }

                    }

                }
            } while (!CheckIfFinishedCombat());
            int result = GetResult();
            battleData.status = result;
            return battleData;
        }

        public BattleProgess ProgessBattleSingleOpponentTarget(BaseCharacter currentCharacter, int turn, int order)
        {
            BattleProgess battleProgress = new BattleProgess();
            battleProgress.turn = turn;
            battleProgress.order = order;
            battleProgress.type = "Attack";
            battleProgress.target = new List<BattleUnit>();
            if (userCharacters.Where(character => character._id == currentCharacter._id).FirstOrDefault() != null)
            {
                BaseCharacter targetCharacter = GetSingleTarget(currentCharacter, opponentCharacters, CONST_COMBAT.SET_TARGET_TYPE.NEAREST_TARGET);
                ICombatLogic combatLogic = new CombatLogic();
                RaceInfo raceInfo = BattleLogic.GetRaceInfo(currentCharacter.race);
                if (raceInfo.counterRaces.Contains(targetCharacter.race))
                {
                    combatLogic = new IncreaseDamage(combatLogic, 10);
                }
                if (raceInfo.couterByRaces.Contains(targetCharacter.race))
                {
                    combatLogic = new DecreaseDamage(combatLogic, 10);
                }
                targetCharacter.hp -= combatLogic.CalculateDamage(currentCharacter.atk);
                battleProgress.attacker = new BattleUnit(currentCharacter.atk, currentCharacter.def, currentCharacter.speed, currentCharacter.hp, currentCharacter._id, "OurSide");
                battleProgress.target.Add(new BattleUnit(targetCharacter.atk, targetCharacter.def, targetCharacter.speed, targetCharacter.hp, targetCharacter._id, "OpposingSide"));
            }
            else if (opponentCharacters.Where(character => character._id == currentCharacter._id).FirstOrDefault() != null)
            {
                BaseCharacter targetCharacter = GetSingleTarget(currentCharacter, userCharacters, CONST_COMBAT.SET_TARGET_TYPE.NEAREST_TARGET);
                ICombatLogic combatLogic = new CombatLogic();
                RaceInfo raceInfo = BattleLogic.GetRaceInfo(currentCharacter.race);
                if (raceInfo.counterRaces.Contains(targetCharacter.race))
                {
                    combatLogic = new IncreaseDamage(combatLogic, 10);
                }
                if (raceInfo.couterByRaces.Contains(targetCharacter.race))
                {
                    combatLogic = new DecreaseDamage(combatLogic, 10);
                }
                targetCharacter.hp -= combatLogic.CalculateDamage(currentCharacter.atk);
                battleProgress.attacker = new BattleUnit(currentCharacter.atk, currentCharacter.def, currentCharacter.speed, currentCharacter.hp, currentCharacter._id, "OpposingSide");
                battleProgress.target.Add(new BattleUnit(targetCharacter.atk, targetCharacter.def, targetCharacter.speed, targetCharacter.hp, targetCharacter._id, "OurSide"));
            }
            return battleProgress;
        }

        public BattleProgess ProgessBattleMultipleOpponentTarget(BaseCharacter currentCharacter, int turn, int order)
        {
            BattleProgess battleProgress = new BattleProgess();
            battleProgress.turn = turn;
            battleProgress.order = order;
            battleProgress.type = "Attack";
            battleProgress.target = new List<BattleUnit>();
            if (userCharacters.Where(character => character._id == currentCharacter._id).FirstOrDefault() != null)
            {
                List<BaseCharacter> targetCharacters = GetMultipleTarget(currentCharacter, opponentCharacters, CONST_COMBAT.SET_TARGET_TYPE.ROW_TARGETS);
                targetCharacters.ForEach(targetCharacter =>
                {
                    targetCharacter.hp = GetUpdateHealth(targetCharacter.hp, CONST_COMBAT.CAST_TYPE.ATTACK, currentCharacter.atk);
                    battleProgress.attacker = new BattleUnit(currentCharacter.atk, currentCharacter.def, currentCharacter.speed, currentCharacter.hp, currentCharacter._id, "OurSide");
                    battleProgress.target.Add(new BattleUnit(targetCharacter.atk, targetCharacter.def, targetCharacter.speed, targetCharacter.hp, targetCharacter._id, "OpposingSide"));
                });
            }
            else if (opponentCharacters.Where(character => character._id == currentCharacter._id).FirstOrDefault() != null)
            {
                List<BaseCharacter> targetCharacters = GetMultipleTarget(currentCharacter, userCharacters, CONST_COMBAT.SET_TARGET_TYPE.ROW_TARGETS);
                targetCharacters.ForEach(targetCharacter =>
                {
                    targetCharacter.hp = GetUpdateHealth(targetCharacter.hp, CONST_COMBAT.CAST_TYPE.ATTACK, currentCharacter.atk);
                    battleProgress.attacker = new BattleUnit(currentCharacter.atk, currentCharacter.def, currentCharacter.speed, currentCharacter.hp, currentCharacter._id, "OpposingSide");
                    battleProgress.target.Add(new BattleUnit(targetCharacter.atk, targetCharacter.def, targetCharacter.speed, targetCharacter.hp, targetCharacter._id, "OurSide"));
                });

            }
            return battleProgress;
        }

        public int GetResult()
        {
            int result = 0;
            if (opponentCharacters.Where(character => character.hp > 0).Count() == 0 && userCharacters.Where(character => character.hp > 0).Count() > 0)
            {
                result = 1;
            }
            else if (userCharacters.Where(character => character.hp > 0).Count() == 0 && opponentCharacters.Where(character => character.hp > 0).Count() > 0)
            {
                result = -1;
            }
            else
            {
                result = 0;
            }
            return result;
        }

        public bool CheckIfFinishedCombat()
        {
            bool isFinished = false;
            if (userCharacters.Where(character => character.hp > 0).Count() == 0 || opponentCharacters.Where(character => character.hp > 0).Count() == 0)
            {
                isFinished = true;
            }
            else
            {
                isFinished = false;
            }
            return isFinished;
        }

        public BaseCharacter GetSingleTarget(BaseCharacter character, List<BaseCharacter> targetCharacters, CONST_COMBAT.SET_TARGET_TYPE setTargetType)
        {
            targetCharacters = targetCharacters.Where(ch => ch.hp > 0).ToList();
            int charPos = character.position;
            DistanceWithCharacter distanceWithCharacter = new DistanceWithCharacter();
            for (int i = 0; i < targetCharacters.Count; i++)
            {
                int tPosition = targetCharacters[i].position;
                int distance = (charPos / 3 + tPosition / 3) + (tPosition % 3);
                if (i == 0)
                {
                    distanceWithCharacter.distance = distance;
                    distanceWithCharacter.character = targetCharacters[i];
                }
                else
                {
                    if (setTargetType == CONST_COMBAT.SET_TARGET_TYPE.NEAREST_TARGET)
                    {
                        if (distance < distanceWithCharacter.distance)
                        {
                            distanceWithCharacter.distance = distance;
                            distanceWithCharacter.character = targetCharacters[i];
                        }
                    }
                    else if (setTargetType == CONST_COMBAT.SET_TARGET_TYPE.FARTHEST_TARGET)
                    {
                        if (distance > distanceWithCharacter.distance)
                        {
                            distanceWithCharacter.distance = distance;
                            distanceWithCharacter.character = targetCharacters[i];
                        }
                    }
                }
            }
            return distanceWithCharacter.character;
        }

        public List<BaseCharacter> GetMultipleTarget(BaseCharacter character, List<BaseCharacter> targetCharacters, CONST_COMBAT.SET_TARGET_TYPE setTargetType)
        {
            targetCharacters = targetCharacters.Where(ch => ch.hp > 0).ToList();
            int charPos = character.position;
            List<BaseCharacter> characters = new List<BaseCharacter>();
            for (int i = 0; i < targetCharacters.Count; i++)
            {
                int tPosition = targetCharacters[i].position;
                if (setTargetType == CONST_COMBAT.SET_TARGET_TYPE.ALL_TARGETS)
                {
                    characters = targetCharacters;
                }
                else if (setTargetType == CONST_COMBAT.SET_TARGET_TYPE.ROW_TARGETS)
                {
                    BaseCharacter nearestOpponent = GetSingleTarget(character, targetCharacters, CONST_COMBAT.SET_TARGET_TYPE.NEAREST_TARGET);
                    switch (nearestOpponent.position)
                    {
                        case 0:
                        case 3:
                        case 6:
                            characters = targetCharacters.Where(tChar => tChar.position == 0 || tChar.position == 3 || tChar.position == 6).ToList();
                            break;
                        case 1:
                        case 4:
                        case 7:
                            characters = targetCharacters.Where(tChar => tChar.position == 1 || tChar.position == 4 || tChar.position == 7).ToList();
                            break;
                        case 2:
                        case 5:
                        case 8:
                            characters = targetCharacters.Where(tChar => tChar.position == 2 || tChar.position == 5 || tChar.position == 8).ToList();
                            break;
                    }
                }
            }
            return characters;
        }

        public int GetUpdateHealth(int currentHealth, CONST_COMBAT.CAST_TYPE castType, int value)
        {
            switch (castType)
            {
                case CONST_COMBAT.CAST_TYPE.ATTACK:
                    currentHealth -= value;
                    if (currentHealth < 0) currentHealth = 0;
                    break;
                case CONST_COMBAT.CAST_TYPE.BUFF:
                    currentHealth += value;
                    break;
            }
            return currentHealth;
        }
    }
}
