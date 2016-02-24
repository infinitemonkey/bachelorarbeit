﻿using System;
using System.Collections.Generic;
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
            IFile file = await FileSystem.Current.LocalStorage.GetFileAsync("test.json");
            return JsonConvert.DeserializeObject<Configuration>(await file.ReadAllTextAsync());
        }
    }
}

