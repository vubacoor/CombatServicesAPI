namespace CombatServiceAPI.Characters
{
    public class CharacterBuilder : ICharacterBuilder
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

        public CharacterBuilder AddAtk(int atk)
        {
            this.atk = atk;
            return this;
        }

        public CharacterBuilder AddBaseKey(string baseKey)
        {
            this.baseKey = baseKey;
            return this;
        }

        public CharacterBuilder AddContractAddress(string contractAddress)
        {
            this.contractAddress = contractAddress;
            return this;
        }

        public CharacterBuilder AddDef(int def)
        {
            this.def = def;
            return this;
        }

        public CharacterBuilder AddHp(int hp)
        {
            this.hp = hp;
            return this;
        }

        public CharacterBuilder AddId(string _id)
        {
            this._id = _id;
            return this;
        }

        public CharacterBuilder AddItemList(string[] itemList)
        {
            this.itemList = itemList;
            return this;
        }

        public CharacterBuilder AddKey(string key)
        {
            this.key = key;
            return this;
        }

        public CharacterBuilder AddLevel(int level)
        {
            this.level = level;
            return this;
        }


        public CharacterBuilder AddNftId(string nftId)
        {
            this.nftId = nftId;
            return this;
        }

        public CharacterBuilder AddPosition(int position)
        {
            this.position = position;
            return this;
        }

        public CharacterBuilder AddRace(string race)
        {
            this.race = race;
            return this;
        }

        public CharacterBuilder AddSpeed(int speed)
        {
            this.speed = speed;
            return this;
        }

        public CharacterBuilder AddStatus(string status)
        {
            this.status = status;
            return this;
        }


        public Character Build()
        {
            return new Character(_id, key, baseKey, status, itemList, atk, def, speed, hp, level, contractAddress, nftId, position, race);
        }
    }
}
