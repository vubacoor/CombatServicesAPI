using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CombatServiceAPI.Modules;
using CombatServiceAPI.Models;
using CombatServiceAPI.Passive.Models;
using CombatServiceAPI.Characters;
using CombatServiceAPI.Model;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;

namespace CombatServiceAPI.Controllers
{
    [Route("api/v0.0.1/[controller]")]
    [ApiController]
    public class CombatController : ControllerBase
    {
        [HttpPost]
        public async Task<BattleData> GetCombat(GetBattleInput battleInput)
        {
            List<Character> userCharacters = battleInput.userCharacters;
            userCharacters[0].baseStat = battleInput.userCharacters[0].baseStat;
            List<Character> opponentCharacters = battleInput.opponentCharacters;
            Dictionary<string, Dictionary<string, Effect>> effects = await GetEffectsConfig();
            Battle battle = new Battle(userCharacters, opponentCharacters, effects);
            BattleData battleData = battle.GetCombatData();
            return battleData;
        }
        public async Task<Dictionary<string, Dictionary<string, Effect>>> GetEffectsConfig()
        {
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync("https://ipfs.pantograph.app/ipfs/Qmf5xVyTJWB17YHtJ3agybEfNVY7R5QFGsUieeLhD6Q5RU"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Dictionary<string, Dictionary<string, Effect>> effects = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, Effect>>>(apiResponse);
                    return effects;
                }
            }
        }
        [HttpGet("test")]
        public string TestGetCombat()
        {
            return "haha";
        }
    }
}
