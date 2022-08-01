using System.Collections.Generic;

namespace CombatServiceAPI.Modules
{

    public class RaceInfo
    {
        public List<string> couterByRaces;
        public List<string> counterRaces;
    }
    public class BattleLogic
    {
        public static RaceInfo GetRaceInfo(string race)
        {
            RaceInfo raceInfo = new RaceInfo();
            raceInfo.couterByRaces = new List<string>();
            raceInfo.counterRaces = new List<string>();

            switch (race)
            {
                case "Guardian":
                    raceInfo.couterByRaces.Add("Imperial");
                    raceInfo.counterRaces.Add("Ghost");
                    break;
                case "Ghost":
                    raceInfo.couterByRaces.Add("Guardian");
                    raceInfo.counterRaces.Add("Imperial");
                    break;
                case "Imperial":
                    raceInfo.couterByRaces.Add("Ghost");
                    raceInfo.counterRaces.Add("Guardian");
                    break;
                default:
                    break;
            }

            return raceInfo;
        }
    }
}
