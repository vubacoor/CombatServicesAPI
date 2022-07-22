namespace CombatServiceAPI.Model
{
    public class BattleUnit
    {
        public int atk { get; set; }
        public int def { get; set; }
        public int speed { get; set; }
        public int hp { get; set; }
        public string _id { get; set; }
        public string faction { get; set; }
        public BattleUnit(int _atk, int _def, int _speed, int _hp, string __id, string _faction)
        {
            atk = _atk;
            def = _def;
            speed = _speed;
            hp = _hp;
            _id = __id;
            faction = _faction;
        }
    }
}
