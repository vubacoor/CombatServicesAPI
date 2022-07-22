namespace CombatServiceAPI.Characters
{
    public interface ICharacterBuilder
    {
        CharacterBuilder AddId(string _id);
        CharacterBuilder AddKey(string key);
        CharacterBuilder AddBaseKey(string baseKey);
        CharacterBuilder AddStatus(string status);
        CharacterBuilder AddItemList(string[] itemList);
        CharacterBuilder AddAtk(int atk);
        CharacterBuilder AddDef(int def);
        CharacterBuilder AddSpeed(int speed);
        CharacterBuilder AddHp(int hp);
        CharacterBuilder AddLevel(int level);
        CharacterBuilder AddContractAddress(string contractAddress);
        CharacterBuilder AddNftId(string nftId);
        CharacterBuilder AddPosition(int position);
        CharacterBuilder AddRace(string race);
        Character Build();
    }
}
