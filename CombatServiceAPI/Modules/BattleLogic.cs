using System;
using System.Collections.Generic;
using CombatServiceAPI.Characters;
using CombatServiceAPI.Models;
using CombatServiceAPI.Passive.Models;
using System.Linq;

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
        public static ElementInfo GetElementInfo(string race)
        {
            ElementInfo elementInfo = new ElementInfo();

            switch (race)
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

        public static CONST_COMBAT.DISASTER_TYPE CheckDisater(float luck)
        {
            CONST_COMBAT.DISASTER_TYPE disaster = CONST_COMBAT.DISASTER_TYPE.NONE;
            double disasterRate = luck * 0.5d;
            double percentTriggerDisaster = s_Random.NextDouble();
            if (percentTriggerDisaster <= disasterRate)
            {
                double percentDecideDisasterType = s_Random.NextDouble();
                if (percentDecideDisasterType < 0.3)
                {
                    disaster = CONST_COMBAT.DISASTER_TYPE.EARTH_QUAKE;
                }
                else if (0.3 <= percentDecideDisasterType && percentDecideDisasterType < 0.6)
                {
                    disaster = CONST_COMBAT.DISASTER_TYPE.STORM;
                }
                else if (0.6 <= percentDecideDisasterType && percentDecideDisasterType < 0.9)
                {
                    disaster = CONST_COMBAT.DISASTER_TYPE.LIGHTNING_STRIKE;
                }
                else
                {
                    disaster = CONST_COMBAT.DISASTER_TYPE.GOD_WILL;
                }
            }
            return disaster;
        }

        public static bool CheckIfCanTriggerEffect(int cost, int rate, int turn, int stackable)
        {
            bool canTrigger;
            if (cost == 0)
            {
                if (turn <= stackable || stackable == -1 || stackable == 0)
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
                if (turn % (cost + 1) == 0)
                {
                    canTrigger = true;
                }
                else
                {
                    canTrigger = false;
                }

            }
            return canTrigger;
        }
        public static List<Character> GetTargets(string targetType, List<Character> userCharacters, List<Character> opponentCharacters, Character caster, string casterSide)
        {
            List<Character> target = new List<Character>();
            switch (targetType)
            {
                case "SELF":
                    target.Add(caster);
                    break;
                case "SINGLE_ENEMY_CLOSEST":
                    if (casterSide == "user") target.Add(BattleLogic.GetSingleTarget(caster, opponentCharacters, "CLOSET"));
                    if (casterSide == "opponent") target.Add(BattleLogic.GetSingleTarget(caster, userCharacters, "CLOSET"));
                    break;
                case "SINGLE_ALLY_CLOSEST":
                    if (casterSide == "user") target.Add(BattleLogic.GetSingleTarget(caster, userCharacters, "CLOSET"));
                    if (casterSide == "opponent") target.Add(BattleLogic.GetSingleTarget(caster, opponentCharacters, "CLOSET"));
                    break;
                case "SINGLE_ENEMY_FARTHEST":
                    if (casterSide == "user") target.Add(BattleLogic.GetSingleTarget(caster, opponentCharacters, "FARTHEST"));
                    if (casterSide == "opponent") target.Add(BattleLogic.GetSingleTarget(caster, userCharacters, "FARTHEST"));
                    break;
                case "SINGLE_ALLY_FARTHEST":
                    if (casterSide == "user") target.Add(BattleLogic.GetSingleTarget(caster, userCharacters, "FARTHEST"));
                    if (casterSide == "opponent") target.Add(BattleLogic.GetSingleTarget(caster, opponentCharacters, "FARTHEST"));
                    break;
                case "ROW_ENEMY_CLOSEST":
                    if (casterSide == "user") target = BattleLogic.GetMultipleTarget(caster, opponentCharacters, CONST_COMBAT.SET_TARGET_TYPE.ROW_TARGETS);
                    if (casterSide == "opponent") target = BattleLogic.GetMultipleTarget(caster, userCharacters, CONST_COMBAT.SET_TARGET_TYPE.ROW_TARGETS);
                    break;
                case "COLUMN_ENEMY_CLOSEST":
                    if (casterSide == "user") target = BattleLogic.GetMultipleTarget(caster, opponentCharacters, CONST_COMBAT.SET_TARGET_TYPE.COLUMN_TARGETS);
                    if (casterSide == "opponent") target = BattleLogic.GetMultipleTarget(caster, userCharacters, CONST_COMBAT.SET_TARGET_TYPE.COLUMN_TARGETS);
                    break;
                case "COLUMN_ALLY_CLOSEST":
                    if (casterSide == "user") target = BattleLogic.GetMultipleTarget(caster, userCharacters, CONST_COMBAT.SET_TARGET_TYPE.COLUMN_TARGETS);
                    if (casterSide == "opponent") target = BattleLogic.GetMultipleTarget(caster, opponentCharacters, CONST_COMBAT.SET_TARGET_TYPE.COLUMN_TARGETS);
                    break;
                case "ALL_ENEMY":
                    if (casterSide == "user") target = BattleLogic.GetMultipleTarget(caster, opponentCharacters, CONST_COMBAT.SET_TARGET_TYPE.ALL_TARGETS);
                    if (casterSide == "opponent") target = BattleLogic.GetMultipleTarget(caster, userCharacters, CONST_COMBAT.SET_TARGET_TYPE.ALL_TARGETS);
                    break;
                case "ALL_ALLY":
                    if (casterSide == "user") target = BattleLogic.GetMultipleTarget(caster, userCharacters, CONST_COMBAT.SET_TARGET_TYPE.ALL_TARGETS);
                    if (casterSide == "opponent") target = BattleLogic.GetMultipleTarget(caster, opponentCharacters, CONST_COMBAT.SET_TARGET_TYPE.ALL_TARGETS);
                    break;
                case "RANDOM_ENEMY":
                    if (casterSide == "user") target.Add(GetRandomEnemy(opponentCharacters));
                    if (casterSide == "opponent") target.Add(GetRandomEnemy(userCharacters));
                    break;
            }
            return target;
        }
        public static Character GetRandomEnemy(List<Character> targetCharacters)
        {
            Character targetCharacter;
            Random rnd = new Random();
            int randomCharIndex = rnd.Next(0, targetCharacters.Count) - 1;
            targetCharacter = targetCharacters[randomCharIndex];
            return targetCharacter;
        }
        public static Character GetSingleTarget(Character character, List<Character> targetCharacters, string setTargetType)
        {
            targetCharacters = targetCharacters.Where(tCharacter => tCharacter._id != character._id).ToList();
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
                    if (setTargetType == "CLOSET")
                    {
                        if (distance < distanceWithCharacter.distance)
                        {
                            distanceWithCharacter.distance = distance;
                            distanceWithCharacter.character = targetCharacters[i];
                        }
                    }
                    else if (setTargetType == "FARTHEST")
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
        public static List<Character> GetMultipleTarget(Character character, List<Character> targetCharacters, CONST_COMBAT.SET_TARGET_TYPE setTargetType)
        {
            targetCharacters = targetCharacters.ToList();
            int charPos = character.position;
            List<Character> characters = new List<Character>();
            for (int i = 0; i < targetCharacters.Count; i++)
            {
                int tPosition = targetCharacters[i].position;
                if (setTargetType == CONST_COMBAT.SET_TARGET_TYPE.ALL_TARGETS)
                {
                    characters = targetCharacters;
                }
                else if (setTargetType == CONST_COMBAT.SET_TARGET_TYPE.ROW_TARGETS)
                {
                    Character nearestOpponent = BattleLogic.GetSingleTarget(character, targetCharacters, "CLOSET");
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
                else if (setTargetType == CONST_COMBAT.SET_TARGET_TYPE.COLUMN_TARGETS)
                {
                    Character nearestOpponent = BattleLogic.GetSingleTarget(character, targetCharacters, "CLOSET");
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

        public static List<EffectOutput> GetEffectOutput(Effect effect, string targetType, List<Character> userCharacters, List<Character> opponentCharacters, Character caster, string casterSide, int currentTurn)
        {
            List<EffectOutput> effectOutputs = new List<EffectOutput>();
            List<Character> targets = BattleLogic.GetTargets(targetType, userCharacters, opponentCharacters, caster, casterSide);

            if (targets != null && targets.Count > 0)
            {
                if (effect.effectBase == "STAT_CHANGE")
                {
                    targets.ForEach(target =>
                    {
                        target.ApplyEffect(effect, currentTurn);
                        CombatStat targetNewStat = new CombatStat(target.combatStat.atk, target.combatStat.def, target.combatStat.speed, target.combatStat.hp, 0, 0, target.combatStat.crit, target.combatStat.luck);
                        if (effect.expireTurn == -1)
                        {
                            effectOutputs.Add(new EffectOutput(target._id, target._id, effect.effectBase, effect.statEffect, targetNewStat));
                        }
                        else
                        {
                            effectOutputs.Add(new EffectOutput(target._id, target._id, effect.effectBase, effect.statEffect, effect.expireTurn, targetNewStat));
                        }
                    });
                }
                else if (effect.effectBase == "ELEMENT_DAMAGE" || effect.effectBase == "FLAT_DAMAGE")
                {
                    targets.ForEach(target =>
                    {
                        caster.ApplyActiveEffect(effect, target, currentTurn);
                        CombatStat targetNewStat = new CombatStat(target.combatStat.atk, target.combatStat.def, target.combatStat.speed, target.combatStat.hp, target.combatStat.takenHp, 0, target.combatStat.crit, target.combatStat.luck);
                        effectOutputs.Add(new EffectOutput(caster._id, target._id, effect.effectBase, effect.statEffect, targetNewStat));
                    });
                }
                else if (effect.effectBase == "FLAT_RECOVER")
                {
                    targets.ForEach(target =>
                    {
                        caster.ApplyRecoverEffect(effect, target, currentTurn);
                        CombatStat targetNewStat = new CombatStat(target.combatStat.atk, target.combatStat.def, target.combatStat.speed, target.combatStat.hp, target.combatStat.takenHp, 0, target.combatStat.crit, target.combatStat.luck);
                        effectOutputs.Add(new EffectOutput(caster._id, target._id, effect.effectBase, effect.statEffect, targetNewStat));
                    });
                }
            }
            return effectOutputs;
        }
    }
}
