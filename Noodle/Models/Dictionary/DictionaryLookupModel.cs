using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noodle.Models.Dictionary
{
    public class DictionaryLookupModel
    {

        public class Audio
        {
            public string type { get; set; }
            public string file { get; set; }
        }

        public class Meaning
        {
            public string content { get; set; }
            public string type { get; set; }
        }

        public class RootObject
        {
            public string term { get; set; }
            public List<Audio> audio { get; set; }
            public List<Meaning> meanings { get; set; }
        }

    }
}
