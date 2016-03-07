using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace Sidste.CrossFramework.Common.Configuration
{
    public class Configuration
    {
        public Menu Menu { get; set; }

        public LevelLayer LevelLayer { get; set; }

        public OptionLayer OptionLayer { get; set; }

        public static Configuration Load()
        {
            Assembly assembly = typeof(Configuration).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("Sidste.CrossFramework.Common.configuration.json");
            string content;
            using (var reader = new StreamReader(stream))
            {
                content = reader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<Configuration>(content);
        }
    }
}

