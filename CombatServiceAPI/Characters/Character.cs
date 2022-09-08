using System;
using System.Collections.Generic;
using CombatServiceAPI.Passive.Interfaces;
using CombatServiceAPI.Passive.Models;
using CombatServiceAPI.Passive.Decorators;
using CombatServiceAPI.Passive.Base;
using CombatServiceAPI.Models;
using CombatServiceAPI.Services;
using CombatServiceAPI.Modules;
using CombatServiceAPI.Controllers;
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
        public string rarity { get; set; }
        public BaseStat baseStat { get; set; }
        public CombatStat combatStat = new CombatStat(0, 0, 0, 0, 0, 0, 0, 0);

        public CombatStat additionalStat;
        public Dictionary<string, Dictionary<string, Effect>> remainingEffects;
        public EffectController effectController;

        public Character()
        {
        }

        public Character(string _id, int position, string side)
        {
            this._id = _id;
            this.position = position;
            this.side = side;
        }

        public Character(string _id, string key, int position, string rarity, BaseStat baseStat)
        {
            this._id = _id;
            this.key = key;
            this.position = position;
            this.rarity = rarity;
            this.baseStat = baseStat;
        }

        public void ClassifyEffects(Dictionary<string, List<Effect>> characterEffects)
        {
            combatStat = new CombatStat(baseStat.atk, baseStat.def, baseStat.speed, baseStat.hp, 0, 0, baseStat.crit, baseStat.luck);
            effectController = new EffectController();
            effectController.ClassifyEffects(characterEffects);
        }

        public void Attack()
        {
            throw new NotImplementedException();
        }

        public void ApplyEffect(Character target, Effect effect, int currentTurn)
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
            effectController.ApplyEffect(target, effect, baseStatLite, combatStat, rarity, currentTurn);
        }

        public void ApplyRecoverEffect(Effect effect, Character target, int currentTurn)
        {
            effectController.ApplyRecoverEffect(effect, target, currentTurn);
        }
        public void ApplySpecialEffect(Effect effect, Character target)
        {

            switch ((AdditionalEffect)Enum.Parse(typeof(AdditionalEffect), effect.additionalEffect, true))
            {
                case AdditionalEffect.SELF_EXPLOSION:
                    effectController.ApplySelfExplosionEffect(effect, combatStat, target, rarity);
                    break;
                case AdditionalEffect.FIRE_ROAR:
                    effectController.ApplyFireRoarEffect(target, effect);
                    break;
            }
        }

        public void ApplyActiveEffect(Effect effect, Character target, int currentTurn)
        {
            effectController.ApplyActiveEffect(effect, combatStat, rarity, target, currentTurn);
        }
        public void ApplyShieldEffect(Effect effect, Character target, int currentTurn)
        {
            effectController.ApplyShieldEffect(effect, target, currentTurn);
        }
        public bool CheckIfcanUseUltimate(int currentTurn)
        {
            return BattleLogic.CheckIfCanTriggerEffect(effectController.ultimateEffects[0], combatStat, currentTurn);
        }

        public List<Effect> GetOverrideEffectsBaseOnType(Effect prevEffect)
        {
            List<Effect> overrideEffectsBaseOnType = new List<Effect>();
            switch ((EffectBase)Enum.Parse(typeof(EffectBase), prevEffect.effectBase, true))
            {
                case EffectBase.ELEMENT_DAMAGE:
                    overrideEffectsBaseOnType = effectController.overrideEffects.Where(effect => effect.type == EffectType.Active).ToList();
                    break;
                case EffectBase.FLAT_RECOVER:
                case EffectBase.ELEMENT_RECOVER:
                    var flag = 1;
                    overrideEffectsBaseOnType = effectController.overrideEffects.Where(effect => effect.type == EffectType.Buff).ToList();
                    break;
            }
            return overrideEffectsBaseOnType;
        }
    }
}
