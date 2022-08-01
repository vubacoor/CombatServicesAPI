namespace CombatServiceAPI.Characters
{
    public class Eleki : BaseCharacter
    {
        public int lightningBallAmt;
        public override void InitPassive()
        {
            this.def++;
        }
    }
}
