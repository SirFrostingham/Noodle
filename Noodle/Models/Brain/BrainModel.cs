using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Documents;

namespace Noodle.Models.Brain
{
    public class BrainModel
    {
        public BrainModel()
        {
            MySign = (BrainEnums.ZodiacSign)Enum.Parse(typeof(BrainEnums.ZodiacSign), ConfigurationManager.AppSettings.Get("mySign"));
        }

        public BrainEnums.ZodiacSign MySign { get; set; }
    }
}