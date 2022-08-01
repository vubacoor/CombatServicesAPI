using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CombatServiceAPI.Model;
using System.Collections.Generic;
using CombatServiceAPI.Modules;
using CombatServiceAPI.Models;
using CombatServiceAPI.Characters;
using System.Text.Json;
using System.Threading.Tasks;

namespace CombatServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CombatController : ControllerBase
    {
        [HttpPost]
        public BattleData GetCombat(GetBattleInput battleInput)
        {
            List<BaseCharacter> userCharacters = battleInput.userCharacters;
            List<BaseCharacter> opponentCharacters = battleInput.opponentCharacters;
            Battle battle = new Battle(userCharacters, opponentCharacters);
            BattleData data = battle.GetBattleData();
            return data;
        }
        [HttpGet("test")]
        public string TestGetCombat()
        {
            return "haha";
        }
    }
}
