using System.Collections.Generic;
using Noodle.Models.Brain;

namespace Noodle.Managers
{
    public class ThoughtManager
    {
        public List<ThoughtModel> CurrentThoughts { get; set; }
        public List<ThoughtModel> ShortTermThoughts { get; set; }
        public List<ThoughtModel> LongTermThoughts { get; set; }

        public ThoughtManager()
        {
            CurrentThoughts = new List<ThoughtModel>();
            ShortTermThoughts = new List<ThoughtModel>();
            LongTermThoughts = new List<ThoughtModel>();
        }
    }
}