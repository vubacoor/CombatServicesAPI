namespace CombatServiceAPI.Characters
{
    public class Character
    {
        public string _id { get; set; }
        public string key { get; set; }
        public string baseKey { get; set; }
        public string status { get; set; }
        public string[] itemList { get; set; }
        public int atk { get; set; }
        public int def { get; set; }
        public int speed { get; set; }
        public int hp { get; set; }
        public int level { get; set; }
        public string contractAddress { get; set; }
        public string nftId { get; set; }
        public int position { get; set; }
        public string race { get; set; }

        public Character(string id, string key, string baseKey, string status, string[] itemList, int atk, int def, int speed, int hp, int level, string contractAddress, string nftId, int position, string race)
        {
            _id = id;
            this.key = key;
            this.baseKey = baseKey;
            this.status = status;
            this.itemList = itemList;
            this.atk = atk;
            this.def = def;
            this.speed = speed;
            this.hp = hp;
            this.level = level;
            this.contractAddress = contractAddress;
            this.nftId = nftId;
            this.position = position;
            this.race = race;
        }
    }
}
