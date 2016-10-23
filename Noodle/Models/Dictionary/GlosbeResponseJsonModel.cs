using System.Collections.Generic;

namespace Noodle.Models.Dictionary
{
    public class GlosbeResponseJsonModel
    {
        public class Meaning
        {
            public string language { get; set; }
            public string text { get; set; }
        }

        public class Phrase
        {
            public string text { get; set; }
            public string language { get; set; }
        }

        public class Tuc
        {
            public List<Meaning> meanings { get; set; }
            public object meaningId { get; set; }
            public List<int> authors { get; set; }
            public Phrase phrase { get; set; }
        }

        public class __invalid_type__69425
        {
            public string U { get; set; }
            public int id { get; set; }
            public string N { get; set; }
            public string url { get; set; }
        }

        public class __invalid_type__1
        {
            public string U { get; set; }
            public int id { get; set; }
            public string N { get; set; }
            public string url { get; set; }
        }

        public class __invalid_type__60172
        {
            public string U { get; set; }
            public int id { get; set; }
            public string N { get; set; }
            public string url { get; set; }
        }

        public class Authors
        {
            public __invalid_type__69425 __invalid_name__69425 { get; set; }
            public __invalid_type__1 __invalid_name__1 { get; set; }
            public __invalid_type__60172 __invalid_name__60172 { get; set; }
        }

        public class RootObject
        {
            public string result { get; set; }
            public List<Tuc> tuc { get; set; }
            public string phrase { get; set; }
            public string from { get; set; }
            public string dest { get; set; }
            public Authors authors { get; set; }
        }
    }
}