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

namespace CombatServiceAPI.Modules
{
    [TestClass()]
    public class BattleTests : IntergrationTest
    {
        [TestMethod()]
        public void TestIfCanTriggerEffect()
        {
            bool canTrigger = BattleLogic.CheckIfCanTriggerEffect(0, 100, 6, 5);
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
            Character targetCharacter = BattleLogic.GetSingleTarget(userCharacters[0], opponentCharacters, "CLOSET");
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
            Character targetCharacter = BattleLogic.GetSingleTarget(userCharacters[0], opponentCharacters, "FARTHEST");
            Assert.AreEqual("id6", targetCharacter._id);
        }
        [TestMethod()]
        public void TestGetSingleNearestAllyTarget()
        {
            List<Character> userCharacters = new List<Character>();
            userCharacters.Add(new Character("id1", 0, "user"));
            userCharacters.Add(new Character("id2", 1, "user"));
            userCharacters.Add(new Character("id3", 2, "user"));
            Character targetCharacter = BattleLogic.GetSingleTarget(userCharacters[0], userCharacters, "CLOSET");
            Assert.AreEqual("id2", targetCharacter._id);
        }
        [TestMethod()]
        public void TestGetSingleFarthestAllyTarget()
        {
            List<Character> userCharacters = new List<Character>();
            userCharacters.Add(new Character("id1", 0, "user"));
            userCharacters.Add(new Character("id2", 1, "user"));
            userCharacters.Add(new Character("id3", 2, "user"));
            Character targetCharacter = BattleLogic.GetSingleTarget(userCharacters[0], userCharacters, "FARTHEST");
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
            List<Character> targetCharacters = BattleLogic.GetMultipleTarget(userCharacters[0], opponentCharacters, CONST_COMBAT.SET_TARGET_TYPE.ROW_TARGETS);
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
            List<Character> targetCharacters = BattleLogic.GetMultipleTarget(userCharacters[0], opponentCharacters, CONST_COMBAT.SET_TARGET_TYPE.COLUMN_TARGETS);
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
            List<Character> targetCharacters = BattleLogic.GetMultipleTarget(userCharacters[0], opponentCharacters, CONST_COMBAT.SET_TARGET_TYPE.ALL_TARGETS);
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
            getBattleInput.userCharacters.Add(new Character("1", "key1", 0, new BaseStat(50, 200, 50, 100, 1, "Guardian", "Ignis", 0, 0)));
            getBattleInput.opponentCharacters.Add(new Character("2", "key2", 0, new BaseStat(50, 200, 50, 100, 1, "Guardian", "Ignis", 0, 0)));
            string json = JsonSerializer.Serialize(getBattleInput);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await TestClient.PostAsync("/api/combat", httpContent);
            string responseString = await response.Content.ReadAsStringAsync();
            List<CombatTurn> castResult = JsonSerializer.Deserialize<List<CombatTurn>>(responseString);
            float atk = castResult[3].orders[castResult[castResult.Count - 1].orders.Count - 1].characterStats["1"].atk;
            Assert.AreEqual(60.5f, atk);
        }
        [TestMethod()]
        public async Task TestPassiveAnima_3Turns()
        {
            GetBattleInput getBattleInput = new GetBattleInput();
            getBattleInput.userCharacters = new List<Character>();
            getBattleInput.opponentCharacters = new List<Character>();
            getBattleInput.userCharacters.Add(new Character("1", "key1", 0, new BaseStat(50, 200, 10, 100, 1, "Guardian", "Anima", 0, 0)));
            getBattleInput.opponentCharacters.Add(new Character("2", "key2", 0, new BaseStat(50, 200, 50, 100, 1, "Guardian", "Ignis", 0, 0)));
            string json = JsonSerializer.Serialize(getBattleInput);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await TestClient.PostAsync("/api/combat", httpContent);
            string responseString = await response.Content.ReadAsStringAsync();
            List<CombatTurn> castResult = JsonSerializer.Deserialize<List<CombatTurn>>(responseString);
            float speed = castResult[3].orders[castResult[castResult.Count - 1].orders.Count - 1].characterStats["1"].speed;
            Assert.AreEqual(10.5f, speed);
        }
        [TestMethod()]
        public async Task TestPassiveEarth_3Turns()
        {
            GetBattleInput getBattleInput = new GetBattleInput();
            getBattleInput.userCharacters = new List<Character>();
            getBattleInput.opponentCharacters = new List<Character>();
            getBattleInput.userCharacters.Add(new Character("1", "key1", 0, new BaseStat(50, 200, 10, 100, 1, "Guardian", "Earth", 0, 0)));
            getBattleInput.opponentCharacters.Add(new Character("2", "key2", 0, new BaseStat(50, 200, 50, 100, 1, "Guardian", "Ignis", 0, 0)));
            string json = JsonSerializer.Serialize(getBattleInput);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await TestClient.PostAsync("/api/combat", httpContent);
            string responseString = await response.Content.ReadAsStringAsync();
            List<CombatTurn> castResult = JsonSerializer.Deserialize<List<CombatTurn>>(responseString);
            float hp = castResult[3].orders[castResult[castResult.Count - 1].orders.Count - 1].characterStats["1"].hp;
            float def = castResult[3].orders[castResult[castResult.Count - 1].orders.Count - 1].characterStats["1"].def;
            Assert.AreEqual(105f, hp);
            Assert.AreEqual(215f, def);
        }
        [TestMethod()]
        public async Task TestPassiveAqua_3Turns()
        {
            GetBattleInput getBattleInput = new GetBattleInput();
            getBattleInput.userCharacters = new List<Character>();
            getBattleInput.opponentCharacters = new List<Character>();
            getBattleInput.userCharacters.Add(new Character("1", "key1", 0, new BaseStat(50, 200, 10, 100, 1, "Guardian", "Aqua", 0, 0)));
            getBattleInput.opponentCharacters.Add(new Character("2", "key2", 0, new BaseStat(50, 200, 50, 100, 1, "Guardian", "Ignis", 0, 0)));
            string json = JsonSerializer.Serialize(getBattleInput);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await TestClient.PostAsync("/api/combat", httpContent);
            string responseString = await response.Content.ReadAsStringAsync();
            List<CombatTurn> castResult = JsonSerializer.Deserialize<List<CombatTurn>>(responseString);
            float hp = castResult[3].orders[castResult[castResult.Count - 1].orders.Count - 1].characterStats["1"].hp;
            float def = castResult[3].orders[castResult[castResult.Count - 1].orders.Count - 1].characterStats["1"].def;
            float atk = castResult[3].orders[castResult[castResult.Count - 1].orders.Count - 1].characterStats["1"].atk;
            float spd = castResult[3].orders[castResult[castResult.Count - 1].orders.Count - 1].characterStats["1"].speed;
            Assert.AreEqual(105f, hp);
            Assert.AreEqual(210f, def);
            Assert.AreEqual(52.5f, atk);
            Assert.AreEqual(10.1f, spd);
        }
    }
}
