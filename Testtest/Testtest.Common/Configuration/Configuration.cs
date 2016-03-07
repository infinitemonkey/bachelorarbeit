using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace Testtest.Common.Configuration
{
    public class Configuration
    {
        public Menu Menu { get; set; }

        public LevelLayer LevelLayer { get; set; }

        public OptionLayer OptionLayer { get; set; }

        public static Configuration Load()
        {
            Assembly assembly = typeof(Configuration).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("Testtest.Common.configuration.json");
            string content;
            using (var reader = new StreamReader(stream))
            {
                content = reader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<Configuration>(content);
        }
    }
}

