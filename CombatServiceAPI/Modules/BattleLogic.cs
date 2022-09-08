using System;
using System.Collections.Generic;
using CombatServiceAPI.Characters;
using CombatServiceAPI.Models;
using CombatServiceAPI.Passive.Models;
using System.Linq;
using System.Text.Json;

namespace CombatServiceAPI.Modules
{

    public class ElementInfo
    {
        public string couterByElement;
        public string counterElement;
    }
    public class BattleLogic
    {
        private static Random s_Random = new Random();
        /// <summary>
        /// Get element counter element & counter by element
        /// </summary>
        /// <param name="element">
        /// element of character
        /// </param>
        /// <returns>
        /// element counter element & counter by element
        /// </returns>
        /// 
        private static float RandomNumberBetween(double minValue, double maxValue)
        {
            float randFloat = 0f;
            double range = maxValue - minValue;
            for (int i = 0; i < 10; i++)
            {
                double sample = s_Random.NextDouble();
                double scaled = (sample * range) + minValue;
                randFloat = (float)scaled;
            }
            return randFloat;
        }
        private static int RandomNumberBetween(int minValue, int maxValue)
        {
            int randInt = s_Random.Next(minValue, maxValue);

            return randInt;
        }
        public static ElementInfo GetElementInfo(string element)
        {
            ElementInfo elementInfo = new ElementInfo();

            switch (element)
            {
                case ELEMENT.Aqua:
                    elementInfo.couterByElement = ELEMENT.Eleki;
                    elementInfo.counterElement = ELEMENT.Ignis;
                    break;
                case ELEMENT.Ignis:
                    elementInfo.couterByElement = ELEMENT.Aqua;
                    elementInfo.counterElement = ELEMENT.Plant;
                    break;
                case ELEMENT.Plant:
                    elementInfo.couterByElement = ELEMENT.Ignis;
                    elementInfo.counterElement = ELEMENT.Anima;
                    break;
                case ELEMENT.Anima:
                    elementInfo.couterByElement = ELEMENT.Plant;
                    elementInfo.counterElement = ELEMENT.Earth;
                    break;
                case ELEMENT.Earth:
                    elementInfo.couterByElement = ELEMENT.Anima;
                    elementInfo.counterElement = ELEMENT.Eleki;
                    break;
                case ELEMENT.Eleki:
                    elementInfo.couterByElement = ELEMENT.Earth;
                    elementInfo.counterElement = ELEMENT.Aqua;
                    break;
                default:
                    break;
            }
            return elementInfo;
        }

        /// <summary>
        /// Check if disaster happens
        /// </summary>
        /// <param name="luck">
        /// highest luck of all characters on each side
        /// </param>
        /// <returns>
        /// return disaster type if disaster happen, none if not
        /// </returns>
        public static DISASTER_TYPE CheckDisater(float totalLuck)
        {
            DISASTER_TYPE disaster = DISASTER_TYPE.NONE;
            double disasterRate = totalLuck * 0.5d;
            double percentTriggerDisaster = s_Random.NextDouble();
            if (percentTriggerDisaster <= disasterRate)
            {
                double percentDecideDisasterType = s_Random.NextDouble();
                if (percentDecideDisasterType < 0.3)
                {
                    disaster = DISASTER_TYPE.EARTH_QUAKE;
                }
                else if (0.3 <= percentDecideDisasterType && percentDecideDisasterType < 0.6)
                {
                    disaster = DISASTER_TYPE.STORM;
                }
                else if (0.6 <= percentDecideDisasterType && percentDecideDisasterType < 0.9)
                {
                    disaster = DISASTER_TYPE.LIGHTNING_STRIKE;
                }
                else
                {
                    disaster = DISASTER_TYPE.GOD_WILL;
                }
            }
            return disaster;
        }

        /// <summary>
        /// Check if one effect can trigger
        /// </summary>
        /// <param name="effect">
        /// Effect needs to be checked
        /// </param>
        /// <param name="stat">
        /// combat stat of caster of that effect
        /// </param>
        /// <param name="turn">
        /// current turn
        /// </param>
        /// <returns>
        /// true if can trigger, false if cannot
        /// </returns>
        public static bool CheckIfCanTriggerEffect(Effect effect, CombatStat stat, int turn)
        {
            bool canTrigger;
            if (effect.cost == 0)
            {
                if (turn <= effect.stackable || effect.stackable == -1 || effect.stackable == 0)
                {
                    if (effect.rate == 100)
                    {
                        canTrigger = true;
                    }
                    else
                    {
                        int rand = s_Random.Next(0, 101);
                        if (rand <= effect.rate)
                        {
                            canTrigger = true;
                        }
                        else
                        {
                            canTrigger = false;
                        }
                    }
                    if (effect.additionalEffect != "NULL")
                    {
                        if (effect.additionalEffect == AdditionalEffect.BERSERKER.ToString()
                            || effect.additionalEffect == AdditionalEffect.SELF_EXPLOSION.ToString())
                        {
                            string healthUnderPercentString = JsonSerializer.Serialize(effect.additionalData);
                            float healthUnderPercent = float.Parse(healthUnderPercentString);
                            var flag = stat.hp / 100f * healthUnderPercent;
                            if (stat.hp - stat.takenHp <= stat.hp / 100f * healthUnderPercent)
                            {
                                canTrigger = true;
                            }
                            else
                            {
                                canTrigger = false;
                            }
                        }
                    }
                }
                else
                {
                    canTrigger = false;
                }
            }
            else
            {
                if (turn % (effect.cost + 1) == 0)
                {
                    int rand = s_Random.Next(0, 101);
                    if (rand <= effect.rate)
                    {
                        canTrigger = true;
                    }
                    else
                    {
                        canTrigger = false;
                    }
                }
                else
                {
                    canTrigger = false;
                }

            }
            return canTrigger;
        }
        /// <summary>
        /// Check if can trigger special effect
        /// </summary>
        /// <param name="effect">
        /// Effect needs to be checked
        /// </param>
        /// <param name="caster">
        ///  combat stat of caster of that effect
        /// </param>
        /// <param name="turn">
        /// Current turn
        /// </param>
        /// <returns>
        ///  true if can trigger, false if cannot
        /// </returns>
        public static bool CheckIfCanTriggerSpecialEffect(Effect effect, Character caster, int turn)
        {
            bool canTrigger;
            if (effect.cost == 0)
            {
                if (turn <= effect.stackable || effect.stackable == -1 || effect.stackable == 0)
                {
                    if (effect.rate == 100)
                    {
                        canTrigger = true;
                    }
                    else
                    {
                        int rand = s_Random.Next(0, 101);
                        if (rand <= effect.rate)
                        {
                            canTrigger = true;
                        }
                        else
                        {
                            canTrigger = false;
                        }
                    }
                }
                else
                {
                    canTrigger = false;
                }
            }
            else
            {
                if (turn % (effect.cost + 1) == 0)
                {
                    int rand = s_Random.Next(0, 101);
                    if (rand <= effect.rate)
                    {
                        canTrigger = true;
                    }
                    else
                    {
                        canTrigger = false;
                    }
                }
                else
                {
                    canTrigger = false;
                }

            }
            return canTrigger;
        }

        /// <summary>
        /// Get list of targets of effect
        /// </summary>
        /// <param name="targetType">
        /// Effect's target type
        /// </param>
        /// <param name="userCharacters">
        /// List of all user characters
        /// </param>
        /// <param name="opponentCharacters">
        /// List of all opponent characters
        /// </param>
        /// <param name="caster">
        /// Caster of effect
        /// </param>
        /// <param name="casterSide">
        /// side of caster (opponent or user)
        /// </param>
        /// <returns></returns>
        public static List<Character> GetTargets(string targetType, List<Character> userCharacters, List<Character> opponentCharacters, Character caster, string casterSide)
        {
            List<Character> target = new List<Character>();
            switch ((Target)Enum.Parse(typeof(Target), targetType, true))
            {
                case Target.SELF:
                    target.Add(caster);
                    break;
                case Target.SINGLE_ENEMY_CLOSEST:
                    if (casterSide == "user") target.Add(BattleLogic.GetSingleTarget(caster, opponentCharacters, TARGET_DISTANCE.NEAREST));
                    if (casterSide == "opponent") target.Add(BattleLogic.GetSingleTarget(caster, userCharacters, TARGET_DISTANCE.NEAREST));
                    break;
                case Target.SINGLE_ALLY_CLOSEST:
                    if (casterSide == "user") target.Add(BattleLogic.GetSingleTarget(caster, userCharacters, TARGET_DISTANCE.NEAREST));
                    if (casterSide == "opponent") target.Add(BattleLogic.GetSingleTarget(caster, opponentCharacters, TARGET_DISTANCE.NEAREST));
                    break;
                case Target.SINGLE_ENEMY_FARTHEST:
                    if (casterSide == "user") target.Add(BattleLogic.GetSingleTarget(caster, opponentCharacters, TARGET_DISTANCE.FARTHEST));
                    if (casterSide == "opponent") target.Add(BattleLogic.GetSingleTarget(caster, userCharacters, TARGET_DISTANCE.FARTHEST));
                    break;
                case Target.SINGLE_ALLY_FARTHEST:
                    if (casterSide == "user") target.Add(BattleLogic.GetSingleTarget(caster, userCharacters, TARGET_DISTANCE.FARTHEST));
                    if (casterSide == "opponent") target.Add(BattleLogic.GetSingleTarget(caster, opponentCharacters, TARGET_DISTANCE.FARTHEST));
                    break;
                case Target.ROW_ENEMY_CLOSEST:
                    if (casterSide == "user") target = BattleLogic.GetMultipleTarget(caster, opponentCharacters, SET_TARGET_TYPE.ROW_TARGETS);
                    if (casterSide == "opponent") target = BattleLogic.GetMultipleTarget(caster, userCharacters, SET_TARGET_TYPE.ROW_TARGETS);
                    break;
                case Target.COLUMN_ENEMY_CLOSEST:
                    if (casterSide == "user") target = BattleLogic.GetMultipleTarget(caster, opponentCharacters, SET_TARGET_TYPE.COLUMN_TARGETS);
                    if (casterSide == "opponent") target = BattleLogic.GetMultipleTarget(caster, userCharacters, SET_TARGET_TYPE.COLUMN_TARGETS);
                    break;
                case Target.COLUMN_ALLY_CLOSEST:
                    if (casterSide == "user") target = BattleLogic.GetMultipleTarget(caster, userCharacters, SET_TARGET_TYPE.COLUMN_TARGETS);
                    if (casterSide == "opponent") target = BattleLogic.GetMultipleTarget(caster, opponentCharacters, SET_TARGET_TYPE.COLUMN_TARGETS);
                    break;
                case Target.ALL_ENEMY:
                    if (casterSide == "user") target = BattleLogic.GetMultipleTarget(caster, opponentCharacters, SET_TARGET_TYPE.ALL_TARGETS);
                    if (casterSide == "opponent") target = BattleLogic.GetMultipleTarget(caster, userCharacters, SET_TARGET_TYPE.ALL_TARGETS);
                    break;
                case Target.ALL_ALLY:
                    if (casterSide == "user") target = BattleLogic.GetMultipleTarget(caster, userCharacters, SET_TARGET_TYPE.ALL_TARGETS);
                    if (casterSide == "opponent") target = BattleLogic.GetMultipleTarget(caster, opponentCharacters, SET_TARGET_TYPE.ALL_TARGETS);
                    break;
                case Target.RANDOM_ENEMY:
                    if (casterSide == "user") target.Add(GetRandomEnemy(opponentCharacters));
                    if (casterSide == "opponent") target.Add(GetRandomEnemy(userCharacters));
                    break;
            }
            return target;
        }

        /// <summary>
        /// Get random enemy
        /// </summary>
        /// <param name="targetCharacters">
        /// Get random target enemy
        /// </param>
        /// <returns>
        /// random target (single)
        /// </returns>
        public static Character GetRandomEnemy(List<Character> targetCharacters)
        {
            Character targetCharacter;
            Random rnd = new Random();
            int randomCharIndex = rnd.Next(1, targetCharacters.Count) - 1;
            targetCharacter = targetCharacters[randomCharIndex];
            return targetCharacter;
        }
        public static Character GetSingleTarget(Character character, List<Character> targetCharacters, TARGET_DISTANCE targetDistance)
        {
            targetCharacters = targetCharacters.Where(tCharacter => tCharacter._id != character._id && !CheckIfDead(tCharacter)).ToList();
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
                    if (targetDistance == TARGET_DISTANCE.NEAREST)
                    {
                        var flag = 1;
                        if (distance < distanceWithCharacter.distance)
                        {
                            distanceWithCharacter.distance = distance;
                            distanceWithCharacter.character = targetCharacters[i];
                        }
                    }
                    else if (targetDistance == TARGET_DISTANCE.FARTHEST)
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

        /// <summary>
        /// Get multile target
        /// </summary>
        /// <param name="character">
        /// caster of effect need to get multile targets
        /// </param>
        /// <param name="targetCharacters">
        /// 
        /// </param>
        /// <param name="setTargetType"></param>
        /// <returns>
        /// List of targets
        /// </returns>
        public static List<Character> GetMultipleTarget(Character character, List<Character> targetCharacters, SET_TARGET_TYPE setTargetType)
        {
            targetCharacters = targetCharacters.Where(tCharacter => !CheckIfDead(tCharacter)).ToList();
            int charPos = character.position;
            List<Character> characters = new List<Character>();
            for (int i = 0; i < targetCharacters.Count; i++)
            {
                int tPosition = targetCharacters[i].position;
                if (setTargetType == SET_TARGET_TYPE.ALL_TARGETS)
                {
                    characters = targetCharacters;
                }
                else if (setTargetType == SET_TARGET_TYPE.ROW_TARGETS)
                {
                    Character nearestOpponent = BattleLogic.GetSingleTarget(character, targetCharacters, TARGET_DISTANCE.NEAREST);
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
                else if (setTargetType == SET_TARGET_TYPE.COLUMN_TARGETS)
                {
                    Character nearestOpponent = BattleLogic.GetSingleTarget(character, targetCharacters, TARGET_DISTANCE.NEAREST);
                    switch (nearestOpponent.position)
                    {
                        case 0:
                        case 1:
                        case 2:
                            characters = targetCharacters.Where(tChar => tChar.position == 0 || tChar.position == 1 || tChar.position == 2).ToList();
                            break;
                        case 3:
                        case 4:
                        case 5:
                            characters = targetCharacters.Where(tChar => tChar.position == 3 || tChar.position == 4 || tChar.position == 5).ToList();
                            break;
                        case 6:
                        case 7:
                        case 8:
                            characters = targetCharacters.Where(tChar => tChar.position == 6 || tChar.position == 7 || tChar.position == 8).ToList();
                            break;
                    }
                }
            }
            return characters;
        }
        /// <summary>
        /// Check if finished combat (combat is finished when all characters of one side all have takenHp <= combat stat hp)
        /// </summary>
        /// <param name="userCharacters">
        /// List of user characters
        /// </param>
        /// <param name="opponentCharacters">
        /// List of opponent characters
        /// </param>
        /// <returns>
        /// true if combat finish, false if not
        /// </returns>
        public static bool CheckIfFinishedCombat(List<Character> userCharacters, List<Character> opponentCharacters)
        {
            bool isFinished = false;
            if (userCharacters.Where(uCharacter => !CheckIfDead(uCharacter)).Count() == 0 || opponentCharacters.Where(oCharacter => !CheckIfDead(oCharacter)).Count() == 0)
            {
                isFinished = true;
            }
            else
            {
                isFinished = false;
            }
            return isFinished;
        }

        /// <summary>
        /// Get result of combat
        /// </summary>
        /// <param name="userCharacters">
        /// List of user characters
        /// </param>
        /// <param name="opponentCharacters">
        /// List of opponent characters
        /// </param>
        /// <returns>
        /// -1 if opponent win, 1 if user win, 0 if draw
        /// </returns>
        public static int GetResult(List<Character> userCharacters, List<Character> opponentCharacters)
        {
            int result = 0;
            if (opponentCharacters.Where(character => character.combatStat.takenHp < character.combatStat.hp).Count() == 0 && userCharacters.Where(character => character.combatStat.takenHp < character.combatStat.hp).Count() > 0)
            {
                result = 1;
            }
            else if (userCharacters.Where(character => character.combatStat.takenHp < character.combatStat.hp).Count() == 0 && opponentCharacters.Where(character => character.combatStat.takenHp < character.combatStat.hp).Count() > 0)
            {
                result = -1;
            }
            else
            {
                result = 0;
            }
            return result;
        }

        /// <summary>
        /// check if one character dead
        /// </summary>
        /// <param name="character"></param>
        /// <returns>
        /// true if dead, false if not
        /// </returns>
        public static bool CheckIfDead(Character character)
        {
            bool isDead;
            if (character.combatStat.takenHp >= character.combatStat.hp)
            {
                isDead = true;
            }
            else
            {
                isDead = false;
            }
            return isDead;
        }

        public static float GetTakenDamage(Character attackChar, Character attackedChar)
        {
            float takenDamage;
            float atkStat = attackChar.combatStat.atk;
            float defStat = attackedChar.combatStat.def;
            float shieldStat = attackedChar.combatStat.shieldAmt;
            float mitigation = 0;
            float rand = RandomNumberBetween(0.9f, 1.1f);
            takenDamage = (((atkStat * atkStat) / (atkStat + defStat)) * 1 * rand) * mitigation - shieldStat;
            return takenDamage;
        }
    }
}
