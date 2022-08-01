namespace CombatServiceAPI.Characters
{
    public class DecreaseDamage : CombatLogicDecorator
    {
        private readonly int decreaseDamageAmt;
        public DecreaseDamage(ICombatLogic actions, int decreaseDamageAmt) : base(actions)
        {
            this.decreaseDamageAmt = decreaseDamageAmt;
        }

        public override int CalculateDamage(int baseDamage)
        {
            if (base.CalculateDamage(baseDamage) == 0)
            {
                return 0;
            }
            else
            {
                return base.CalculateDamage(baseDamage) - decreaseDamageAmt;
            }
        }
    }
}
