using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PCLStorage;

namespace Testtest.Common
{
	public class OptionLayer
	{
        public string BackgroundImage { get; set; }
        public DefaultButton BackButton { get; set; }
        public Layout Layout { get; set; }
        public Option SoundOption { get; set; }
        public IEnumerable<Option> Options { get; set; }
	}
}

