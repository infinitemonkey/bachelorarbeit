using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Testtest.Common
{
    public class Configuration
    {
        public Menu Menu { get; set; }

        public IEnumerable<Level> Levels { get; set; }

        public IEnumerable<Option> Options { get; set; }

        public static Configuration Load()
        {
            return JsonConvert.DeserializeObject<Configuration>("");
        }
    }
}

