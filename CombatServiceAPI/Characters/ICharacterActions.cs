namespace CombatServiceAPI.Characters
{
    public interface ICharacterActions
    {
        int NormalFight(Character character, Character target);
        int SpecialFight(Character character, Character target);
    }
}
