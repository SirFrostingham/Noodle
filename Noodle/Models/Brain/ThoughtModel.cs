using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noodle.Models.Brain
{
    public class ThoughtModel
    {
        public string Subject { get; set; }
        public string InputPhrase { get; set; }
        public string ReponsePhrase { get; set; }
        public BrainEnums.Emotion Emotion { get; set; }
        public BrainEnums.Alignment Alignment { get; set; }
        public bool IsInapropriate { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
