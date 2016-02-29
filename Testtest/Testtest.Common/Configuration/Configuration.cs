using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PCLStorage;

namespace Testtest.Common
{
    public class Configuration
    {
        public Menu Menu { get; set; }

        public LevelLayer LevelLayer { get; set; }

        public OptionLayer OptionLayer { get; set; }

        public static Configuration Load()
        {
            var assembly = typeof(Configuration).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("Testtest.Common.configuration.json");
            string text = "";
            using (var reader = new System.IO.StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<Configuration>(text);
        }
    }
}

