namespace CombatServiceAPI.Characters
{
    public class Ignis : BaseCharacter
    {
        public int ignisPoint;
        public override void TurnPassive()
        {
            if (this.ignisPoint < 4)
            {
                this.ignisPoint++;
                if (this.ignisPoint > 0)
                {
                    this.atk++;
                    this.def--;
                }
            }
        }
    }
}
