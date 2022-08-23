using System.Collections.Generic;

namespace CombatServiceAPI.Models
{
    public class Ability
    {
        public string type { get; set; }
        public List<string> effectIds { get; set; }
    }
}
