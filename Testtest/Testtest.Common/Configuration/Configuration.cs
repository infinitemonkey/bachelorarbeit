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

        public IEnumerable<Level> Levels { get; set; }

        public IEnumerable<Option> Options { get; set; }

        public static async Task<Configuration> Load()
        {
            var assembly = typeof(Configuration).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("Testtest.Common.test.json");
            string text = "";
            using (var reader = new System.IO.StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }
            //IFile file = await FileSystem.Current.LocalStorage.GetFileAsync("test.json");
            //return JsonConvert.DeserializeObject<Configuration>(await file.ReadAllTextAsync());
            return JsonConvert.DeserializeObject<Configuration>(text);
        }
    }
}

