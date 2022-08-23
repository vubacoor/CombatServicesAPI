public class CONST_COMBAT
{
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
