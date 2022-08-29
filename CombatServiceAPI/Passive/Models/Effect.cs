using System.Collections.Generic;

namespace CombatServiceAPI.Passive.Models
{
    public enum EffectType
    {
        Passive,
        Active,
        Ultimate
    }
    public enum EffectBase
    {
        ELEMENT_DAMAGE,
        FLAT_DAMAGE,
        TRUE_DAMAGE,
        ELEMENT_RECOVER,
        FLAT_RECOVER,
        ELEMENT_SHIELD,
        SHIELD,
        ELEMENT_REFLECT_SHIELD,
        REFLECT_SHIELD,
        STAT_CHANGE,
        STAT_CHANGE_TOTAL,
        REDUCE_DAMAGE_TAKEN
    }
    public enum AmountType
    {
        FLAT,
        PERCENT
    }
    public enum Target
    {
        SELF,
        SINGLE_ENEMY_CLOSEST,
        SINGLE_ALLY_CLOSEST,
        SINGLE_ENEMY_FARTHEST,
        SINGLE_ALLY_FARTHEST,
        ROW_ENEMY_CLOSEST,
        ROW_ALLY_CLOSEST,
        COLUMN_ENEMY_CLOSEST,
        COLUMN_ALLY_CLOSEST,
        ALL_ENEMY,
        ALL_ALLY,
        OVERRIDE_TARGET
    }
    public enum Rarity
    {
        Comon,
        Uncomon,
        Rare,
        Epic,
        Legendary,
        Emperor
    }
    public class Effect
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string effectBase { get; set; }
        public dynamic amount { get; set; }
        public string amountType { get; set; }
        public string target { get; set; }
        public string statEffect { get; set; }
        public int expireTurn { get; set; }
        public int stackable { get; set; }
        public int rate { get; set; }
        public int cost { get; set; }
        public string additionalEffect { get; set; }
        public dynamic additionalData { get; set; }
        public string phaseTrigger { get; set; }

        public Effect(string id, string name, string type, string effectBase, dynamic amount, string amountType, string target, string statEffect, int expireTurn, int stackable, int rate, int cost, string additionalEffect, dynamic additionalData, string phaseTrigger)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.effectBase = effectBase;
            this.amount = amount;
            this.amountType = amountType;
            this.target = target;
            this.statEffect = statEffect;
            this.expireTurn = expireTurn;
            this.stackable = stackable;
            this.rate = rate;
            this.cost = cost;
            this.additionalEffect = additionalEffect;
            this.additionalData = additionalData;
            this.phaseTrigger = phaseTrigger;
        }
    }
}
