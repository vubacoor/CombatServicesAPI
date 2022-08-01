using System;

namespace CombatServiceAPI.Characters
{
    class SkillEffect
    {

    }
    public class CombatLogic : ICombatLogic
    {
        public int CalculateDamage(int baseDamage)
        {
            return baseDamage;
        }

        public int Calculate()
        {
            return 0;
        }

        public void Passive()
        {
            Console.WriteLine("Passive");
        }
    }
}
