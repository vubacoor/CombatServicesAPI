namespace CombatServiceAPI.Passive.Models
{
    public class CombatStat
    {
        public float atk { get; set; }
        public float def { get; set; }
        public float speed { get; set; }
        public float hp { get; set; }
        public float takenHp { get; set; }
        public float reduceDamage { get; set; }
        public float crit { get; set; }
        public float luck { get; set; }
        public CombatStat()
        {
        }
        public CombatStat(float atk, float def, float speed, float hp, float takenHp, float reduceDamage, float crit, float luck)
        {
            this.atk = atk;
            this.def = def;
            this.speed = speed;
            this.hp = hp;
            this.takenHp = takenHp;
            this.reduceDamage = reduceDamage;
            this.crit = crit;
            this.luck = luck;
        }
    }
}
