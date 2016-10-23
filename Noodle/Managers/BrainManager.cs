using Noodle.Models.Brain;

namespace Noodle.Managers
{
    public class BrainManager
    {
        public BrainModel BrainModel { get; set; }

        public BrainManager()
        {
            //TODO: Use, change or throw away
            BrainModel = new BrainModel();
        }
    }
}