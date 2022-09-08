public enum SET_TARGET_TYPE
{
    NEAREST_TARGET,
    FARTHEST_TARGET,

    ROW_TARGETS,
    COLUMN_TARGETS,
    ALL_TARGETS,
}

public enum CAST_TYPE
{
    ATTACK,
    BUFF,
}

public enum DISASTER_TYPE
{
    EARTH_QUAKE,
    STORM,
    LIGHTNING_STRIKE,
    GOD_WILL,
    NONE
}
public class ELEMENT
{
    public const string Ignis = "Ignis";
    public const string Plant = "Plant";
    public const string Anima = "Anima";
    public const string Earth = "Earth";
    public const string Eleki = "Eleki";
    public const string Aqua = "Aqua";

}

public class StatEffect
{
    public const string ATK_OWNER = "ATK_OWNER";
    public const string ATK_TARGET = "ATK_TARGET";

    public const string DEF_OWNER = "DEF_OWNER";
    public const string DEF_TARGET = "DEF_TARGET";

    public const string DAMAGE_OWNER = "DAMAGE_OWNER";
    public const string DAMAGE_TARGET = "DAMAGE_TARGET";

    public const string HP_OWNER = "HP_OWNER";
    public const string HP_TARGET = "HP_TARGET";

    public const string SPD_OWNER = "SPD_OWNER";
    public const string SPD_TARGET = "SPD_TARGET";

    public const string CRIT_OWNER = "CRIT_OWNER";
    public const string CRIT_TARGET = "CRIT_TARGET";

    public const string LUCK_OWNER = "LUCK_OWNER";
    public const string LUCK_TARGET = "LUCK_TARGET";
}

public class EffectType
{
    public const string Passive = "Passive";
    public const string Active = "Active";
    public const string Ultimate = "Ultimate";
    public const string Buff = "Buff";
}

public class PhaseTrigger
{
    public const string START_GAME = "START_GAME";
    public const string START_TURN = "START_TURN";
    public const string START_CHARACTER = "START_CHARACTER";
    public const string ACTION = "ACTION";
    public const string END_CHARACTER = "END_CHARACTER";
    public const string END_TURN = "END_TURN";
    public const string END_GAME = "END_GAME";
    public const string OVERRIDE_PHASE = "OVERRIDE_PHASE";
    public const string START_ORDER = "START_ORDER";
    public const string END_ORDER = "END_ORDER";
}

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary,
    Emperor
}
public enum EffectBase
{
    ELEMENT_DAMAGE,
    FLAT_DAMAGE,
    TRUE_DAMAGE,
    ELEMENT_RECOVER,
    FLAT_RECOVER,
    FLAT_SHIELD,
    ELEMENT_SHIELD,
    SHIELD,
    ELEMENT_REFLECT_SHIELD,
    REFLECT_SHIELD,
    STAT_CHANGE,
    STAT_CHANGE_TOTAL,
    REDUCE_DAMAGE_TAKEN,
    BONDING_ALLY_NEAREST
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
    OVERRIDE_TARGET,
    RANDOM_ENEMY
}

public enum AdditionalEffect
{
    BONDING,
    CHAIN_ATTACK,
    RELEASE_POWER,
    RECOVER_COMBINE,
    SELF_EXPLOSION,
    SHIELD_COMBINE,
    BERSERKER,
    FIRE_ROAR,
    STAT_MAX_CHANGE_PERCENT
}

public enum TARGET_DISTANCE
{
    NEAREST,
    FARTHEST
}