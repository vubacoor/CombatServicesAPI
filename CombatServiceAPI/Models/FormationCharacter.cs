namespace CombatServiceAPI
{
    public class FormationCharacter
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

        public int enegery { get; set; }
    }
}
