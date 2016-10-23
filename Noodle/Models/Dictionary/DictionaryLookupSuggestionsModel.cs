using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noodle.Models.Dictionary
{
    public class DictionaryLookupSuggestionsModel
    {
        public class RootObject
        {
            public List<string> suggestions { get; set; }
        }
    }
}
