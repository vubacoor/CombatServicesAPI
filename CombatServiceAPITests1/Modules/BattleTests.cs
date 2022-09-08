using Microsoft.VisualStudio.TestTools.UnitTesting;
using CombatServiceAPI.Modules;
using CombatServiceAPI.Characters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using System.Net.Http;
using CombatServiceAPI.Models;
using System.Text.Json;
using CombatServiceAPI.Passive.Models;
using CombatServiceAPI.Model;

namespace CombatServiceAPI.Modules
{
    [TestClass()]
    public class BattleTests : IntergrationTest
    {
        [TestMethod()]
        public void TestIfCanTriggerEffect()
        {
            Effect effect = new Effect(
                "1",
                "Guardian",
                "Passive",
                "STAT_CHANGE",
                10,
                "PERCENT",
                "SELF",
                "DEF_OWNER",
                -1,
                -1,
                100,
                3,
                null,
                null,
                "START_TURN");
            CombatStat combatStat = new CombatStat(100, 100, 100, 100, 0, 0, 0, 0);
            bool canTrigger = BattleLogic.CheckIfCanTriggerEffect(effect, combatStat, 1);
            Assert.AreEqual(false, canTrigger);
        }
        [TestMethod()]
        public void TestGetSingleNearestOpponentTarget()
        {
            List<Character> userCharacters = new List<Character>();
            userCharacters.Add(new Character("id1", 0, "user"));
            userCharacters.Add(new Character("id2", 1, "user"));
            userCharacters.Add(new Character("id3", 2, "user"));
            List<Character> opponentCharacters = new List<Character>();
            opponentCharacters.Add(new Character("id4", 0, "opponent"));
            opponentCharacters.Add(new Character("id5", 1, "opponent"));
            opponentCharacters.Add(new Character("id6", 2, "opponent"));
            Character targetCharacter = BattleLogic.GetSingleTarget(userCharacters[0], opponentCharacters, TARGET_DISTANCE.NEAREST);
            Assert.AreEqual("id4", targetCharacter._id);
        }
        [TestMethod()]
        public void TestGetSingleFarthestOpponentTarget()
        {
            List<Character> userCharacters = new List<Character>();
            userCharacters.Add(new Character("id1", 0, "user"));
            userCharacters.Add(new Character("id2", 1, "user"));
            userCharacters.Add(new Character("id3", 2, "user"));
            List<Character> opponentCharacters = new List<Character>();
            opponentCharacters.Add(new Character("id4", 0, "opponent"));
            opponentCharacters.Add(new Character("id5", 1, "opponent"));
            opponentCharacters.Add(new Character("id6", 2, "opponent"));
            Character targetCharacter = BattleLogic.GetSingleTarget(userCharacters[0], opponentCharacters, TARGET_DISTANCE.FARTHEST);
            Assert.AreEqual("id6", targetCharacter._id);
        }
        [TestMethod()]
        public void TestGetSingleNearestAllyTarget()
        {
            List<Character> userCharacters = new List<Character>();
            userCharacters.Add(new Character("id1", 0, "user"));
            userCharacters.Add(new Character("id2", 1, "user"));
            userCharacters.Add(new Character("id3", 2, "user"));
            Character targetCharacter = BattleLogic.GetSingleTarget(userCharacters[0], userCharacters, TARGET_DISTANCE.NEAREST);
            Assert.AreEqual("id2", targetCharacter._id);
        }
        [TestMethod()]
        public void TestGetSingleFarthestAllyTarget()
        {
            List<Character> userCharacters = new List<Character>();
            userCharacters.Add(new Character("id1", 0, "user"));
            userCharacters.Add(new Character("id2", 1, "user"));
            userCharacters.Add(new Character("id3", 2, "user"));
            Character targetCharacter = BattleLogic.GetSingleTarget(userCharacters[0], userCharacters, TARGET_DISTANCE.FARTHEST);
            Assert.AreEqual("id3", targetCharacter._id);
        }
        [TestMethod()]
        public void TestGetRowOpponentCloset()
        {
            List<Character> userCharacters = new List<Character>();
            userCharacters.Add(new Character("id1", 0, "user"));
            userCharacters.Add(new Character("id2", 1, "user"));
            userCharacters.Add(new Character("id3", 2, "user"));
            List<Character> opponentCharacters = new List<Character>();
            opponentCharacters.Add(new Character("id4", 0, "opponent"));
            opponentCharacters.Add(new Character("id5", 1, "opponent"));
            opponentCharacters.Add(new Character("id6", 2, "opponent"));
            opponentCharacters.Add(new Character("id7", 3, "opponent"));
            opponentCharacters.Add(new Character("id8", 4, "opponent"));
            opponentCharacters.Add(new Character("id9", 5, "opponent"));
            opponentCharacters.Add(new Character("id10", 6, "opponent"));
            List<string> expectedResult = new List<string>();
            expectedResult.Add("id4");
            expectedResult.Add("id7");
            expectedResult.Add("id10");
            bool isNotContains = false;
            List<Character> targetCharacters = BattleLogic.GetMultipleTarget(userCharacters[0], opponentCharacters, SET_TARGET_TYPE.ROW_TARGETS);
            targetCharacters.ForEach(tChar =>
            {
                if (expectedResult.FindIndex(eResult => eResult == tChar._id) == -1)
                {
                    isNotContains = true;
                }
            });
            Assert.AreEqual(false, isNotContains);
        }
        [TestMethod()]
        public void TestGetColumnOpponentCloset()
        {
            List<Character> userCharacters = new List<Character>();
            userCharacters.Add(new Character("id1", 0, "user"));
            userCharacters.Add(new Character("id2", 1, "user"));
            userCharacters.Add(new Character("id3", 2, "user"));
            List<Character> opponentCharacters = new List<Character>();
            opponentCharacters.Add(new Character("id4", 0, "opponent"));
            opponentCharacters.Add(new Character("id5", 1, "opponent"));
            opponentCharacters.Add(new Character("id6", 2, "opponent"));
            opponentCharacters.Add(new Character("id7", 3, "opponent"));
            opponentCharacters.Add(new Character("id8", 4, "opponent"));
            opponentCharacters.Add(new Character("id9", 5, "opponent"));
            opponentCharacters.Add(new Character("id10", 6, "opponent"));
            List<string> expectedResult = new List<string>();
            expectedResult.Add("id4");
            expectedResult.Add("id5");
            expectedResult.Add("id6");
            bool isNotContains = false;
            List<Character> targetCharacters = BattleLogic.GetMultipleTarget(userCharacters[0], opponentCharacters, SET_TARGET_TYPE.COLUMN_TARGETS);
            targetCharacters.ForEach(tChar =>
            {
                if (expectedResult.FindIndex(eResult => eResult == tChar._id) == -1)
                {
                    isNotContains = true;
                }
            });
            Assert.AreEqual(false, isNotContains);
        }
        [TestMethod]
        public void TestGetAllOpponents()
        {
            List<Character> userCharacters = new List<Character>();
            userCharacters.Add(new Character("id1", 0, "user"));
            userCharacters.Add(new Character("id2", 1, "user"));
            userCharacters.Add(new Character("id3", 2, "user"));
            List<Character> opponentCharacters = new List<Character>();
            opponentCharacters.Add(new Character("id4", 0, "opponent"));
            opponentCharacters.Add(new Character("id5", 1, "opponent"));
            opponentCharacters.Add(new Character("id6", 2, "opponent"));
            opponentCharacters.Add(new Character("id7", 3, "opponent"));
            opponentCharacters.Add(new Character("id8", 4, "opponent"));
            opponentCharacters.Add(new Character("id9", 5, "opponent"));
            opponentCharacters.Add(new Character("id10", 6, "opponent"));
            List<string> expectedResult = new List<string>();
            expectedResult.Add("id4");
            expectedResult.Add("id5");
            expectedResult.Add("id6");
            expectedResult.Add("id7");
            expectedResult.Add("id8");
            expectedResult.Add("id9");
            expectedResult.Add("id10");
            bool isNotContains = false;
            List<Character> targetCharacters = BattleLogic.GetMultipleTarget(userCharacters[0], opponentCharacters, SET_TARGET_TYPE.ALL_TARGETS);
            targetCharacters.ForEach(tChar =>
            {
                if (expectedResult.FindIndex(eResult => eResult == tChar._id) == -1)
                {
                    isNotContains = true;
                }
            });
            Assert.AreEqual(false, isNotContains);
        }
        [TestMethod]
        public void TestGetElementInfo()
        {
            ElementInfo ignisElementInfo = BattleLogic.GetElementInfo(ELEMENT.Ignis);
            Assert.AreEqual(ELEMENT.Plant, ignisElementInfo.counterElement);
            Assert.AreEqual(ELEMENT.Aqua, ignisElementInfo.couterByElement);
            ElementInfo aquaElementInfo = BattleLogic.GetElementInfo(ELEMENT.Aqua);
            Assert.AreEqual(ELEMENT.Ignis, aquaElementInfo.counterElement);
            Assert.AreEqual(ELEMENT.Eleki, aquaElementInfo.couterByElement);
        }
        [TestMethod()]
        public async Task TestPassiveIgnis_3Turns()
        {
            GetBattleInput getBattleInput = new GetBattleInput();
            getBattleInput.userCharacters = new List<Character>();
            getBattleInput.opponentCharacters = new List<Character>();
            getBattleInput.userCharacters.Add(new Character("1", "key1", 0, "Common", new BaseStat(50, 200, 50, 100, 1, "Guardian", "Ignis", 0, 0)));
            getBattleInput.opponentCharacters.Add(new Character("2", "key2", 0, "Common", new BaseStat(50, 200, 50, 100, 1, "Guardian", "Ignis", 0, 0)));
            string json = JsonSerializer.Serialize(getBattleInput);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await TestClient.PostAsync("/api/combat", httpContent);
            string responseString = await response.Content.ReadAsStringAsync();
            BattleData castResult = JsonSerializer.Deserialize<BattleData>(responseString);
            float atk = castResult.battleProgress[3].orders[castResult.battleProgress[castResult.battleProgress.Count - 1].orders.Count - 1].characterStats["1"].atk;
            Assert.AreEqual(60.5f, atk);
        }
        [TestMethod()]
        public async Task TestPassiveAnima_3Turns()
        {
            GetBattleInput getBattleInput = new GetBattleInput();
            getBattleInput.userCharacters = new List<Character>();
            getBattleInput.opponentCharacters = new List<Character>();
            getBattleInput.userCharacters.Add(new Character("1", "key1", 0, "Common", new BaseStat(50, 200, 10, 100, 1, "Guardian", "Anima", 0, 0)));
            getBattleInput.opponentCharacters.Add(new Character("2", "key2", 0, "Common", new BaseStat(50, 200, 50, 100, 1, "Guardian", "Ignis", 0, 0)));
            string json = JsonSerializer.Serialize(getBattleInput);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await TestClient.PostAsync("/api/combat", httpContent);
            string responseString = await response.Content.ReadAsStringAsync();
            BattleData castResult = JsonSerializer.Deserialize<BattleData>(responseString);
            float speed = castResult.battleProgress[3].orders[castResult.battleProgress[castResult.battleProgress.Count - 1].orders.Count - 1].characterStats["1"].speed;
            Assert.AreEqual(10.5f, speed);
        }
        [TestMethod()]
        public async Task TestPassiveEarth_3Turns()
        {
            GetBattleInput getBattleInput = new GetBattleInput();
            getBattleInput.userCharacters = new List<Character>();
            getBattleInput.opponentCharacters = new List<Character>();
            getBattleInput.userCharacters.Add(new Character("1", "key1", 0, "Common", new BaseStat(50, 200, 10, 100, 1, "Guardian", "Earth", 0, 0)));
            getBattleInput.opponentCharacters.Add(new Character("2", "key2", 0, "Common", new BaseStat(50, 200, 50, 100, 1, "Guardian", "Ignis", 0, 0)));
            string json = JsonSerializer.Serialize(getBattleInput);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await TestClient.PostAsync("/api/combat", httpContent);
            string responseString = await response.Content.ReadAsStringAsync();
            BattleData castResult = JsonSerializer.Deserialize<BattleData>(responseString);
            float hp = castResult.battleProgress[3].orders[castResult.battleProgress[castResult.battleProgress.Count - 1].orders.Count - 1].characterStats["1"].hp;
            float def = castResult.battleProgress[3].orders[castResult.battleProgress[castResult.battleProgress.Count - 1].orders.Count - 1].characterStats["1"].def;
            Assert.AreEqual(105f, hp);
            Assert.AreEqual(215f, def);
        }
        [TestMethod()]
        public async Task TestPassiveAqua_3Turns()
        {
            GetBattleInput getBattleInput = new GetBattleInput();
            getBattleInput.userCharacters = new List<Character>();
            getBattleInput.opponentCharacters = new List<Character>();
            getBattleInput.userCharacters.Add(new Character("1", "key1", 0, "Common", new BaseStat(50, 200, 10, 100, 1, "Guardian", "Aqua", 0, 0)));
            getBattleInput.opponentCharacters.Add(new Character("2", "key2", 0, "Common", new BaseStat(50, 200, 50, 100, 1, "Guardian", "Ignis", 0, 0)));
            string json = JsonSerializer.Serialize(getBattleInput);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await TestClient.PostAsync("/api/combat", httpContent);
            string responseString = await response.Content.ReadAsStringAsync();
            BattleData castResult = JsonSerializer.Deserialize<BattleData>(responseString);
            float hp = castResult.battleProgress[3].orders[castResult.battleProgress[castResult.battleProgress.Count - 1].orders.Count - 1].characterStats["1"].hp;
            float def = castResult.battleProgress[3].orders[castResult.battleProgress[castResult.battleProgress.Count - 1].orders.Count - 1].characterStats["1"].def;
            float atk = castResult.battleProgress[3].orders[castResult.battleProgress[castResult.battleProgress.Count - 1].orders.Count - 1].characterStats["1"].atk;
            float spd = castResult.battleProgress[3].orders[castResult.battleProgress[castResult.battleProgress.Count - 1].orders.Count - 1].characterStats["1"].speed;
            Assert.AreEqual(105f, hp);
            Assert.AreEqual(210f, def);
            Assert.AreEqual(52.5f, atk);
            Assert.AreEqual(10.1f, spd);
        }
        [TestMethod()]
        public async Task TestDamageGuardianIgnis_Ultimate()
        {
            GetBattleInput getBattleInput = new GetBattleInput();
            getBattleInput.userCharacters = new List<Character>();
            getBattleInput.opponentCharacters = new List<Character>();
            getBattleInput.userCharacters.Add(new Character("user0", "key1", 0, "Common", new BaseStat(50, 200, 10, 100, 1, "Guardian", "Ignis", 0, 0)));
            getBattleInput.opponentCharacters.Add(new Character("enemy0", "key2", 0, "Common", new BaseStat(50, 200, 50, 100, 1, "Guardian", "Earth", 0, 0)));
            getBattleInput.opponentCharacters.Add(new Character("enemy3", "key2", 3, "Common", new BaseStat(50, 200, 50, 100, 1, "Guardian", "Earth", 0, 0)));
            getBattleInput.opponentCharacters.Add(new Character("enemy6", "key2", 6, "Common", new BaseStat(50, 200, 50, 100, 1, "Guardian", "Earth", 0, 0)));
            string json = JsonSerializer.Serialize(getBattleInput);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await TestClient.PostAsync("/api/combat", httpContent);
            string responseString = await response.Content.ReadAsStringAsync();
            BattleData castResult = JsonSerializer.Deserialize<BattleData>(responseString);
            CombatOrder order = castResult.battleProgress[4].orders.First(order => order.characterId == "user0");
            Dictionary<string, CombatStat> characterStats = order.characterStats;
            Character caster = getBattleInput.userCharacters[0];
            float casterBaseAtk = caster.baseStat.atk;
            float amtAtkIncreasePerTurn = (casterBaseAtk * (1f / 100f)) + 3;
            float finalAtk = ((caster.baseStat.atk + (amtAtkIncreasePerTurn * 4)) / 100) * 135;
            Assert.AreEqual(finalAtk, order.characterStats["user0"].atk / 100 * 135);
        }
    }
}
