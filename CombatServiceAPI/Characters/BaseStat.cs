namespace CombatServiceAPI.Characters
{
    public class BaseStat
    {
        public float atk { get; set; }
        public float def { get; set; }
        public float speed { get; set; }
        public float hp { get; set; }
        public float crit { get; set; }
        public float luck { get; set; }
        public float level { get; set; }
        public string race { get; set; }
        public string element { get; set; }
        public BaseStat() { }
        public BaseStat(float atk, float def, float speed, float hp, float level, string race, string element, float crit, float luck)
        {
            this.atk = atk;
            this.def = def;
            this.speed = speed;
            this.hp = hp;
            this.level = level;
            this.race = race;
            this.element = element;
            this.crit = crit;
            this.luck = luck;
        }
    }
}
