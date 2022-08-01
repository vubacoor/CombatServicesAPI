namespace CombatServiceAPI.Characters
{
    public class Plant : BaseCharacter
    {
        public override void TurnPassive()
        {
            this.hp++;
        }
    }
}
