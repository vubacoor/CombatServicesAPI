using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CombatServiceAPI.Modules.Tests
{
    [TestClass()]
    public class BattleTests
    {
        [TestMethod()]
        public void TestIfCanTriggerEffect()
        {
            bool canTrigger = BattleLogic.CheckIfCanTriggerEffect(0, 100, 5, 5);
            Assert.AreEqual(true, canTrigger);
        }
    }
}