namespace CombatServiceAPI.Characters
{
    public class IncreaseDamage : CombatLogicDecorator
    {
        private readonly int increaseDamageAmt;
        public IncreaseDamage(ICombatLogic actions, int increaseDamageAmt) : base(actions)
        {
            this.increaseDamageAmt = increaseDamageAmt;
        }

        public override int CalculateDamage(int baseDamage)
        {
            return base.CalculateDamage(baseDamage) + increaseDamageAmt;
        }
    }
}
