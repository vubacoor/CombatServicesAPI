namespace CombatServiceAPI.Passive.Interfaces
{
    public interface ICombatLogic
    {
        int CalculateDamage(int baseDamage);

        int CalculateSpeed(int baseSpd);
    }
}
