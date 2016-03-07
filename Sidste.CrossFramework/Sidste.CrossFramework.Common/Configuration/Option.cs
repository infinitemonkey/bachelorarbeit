using System.Collections.Generic;

namespace Sidste.CrossFramework.Common.Configuration
{
	public class Option
	{
        public Dictionary<string, string> Text { get; set; }
        public string Key { get; set; }
        public string OnImage { get; set; }
        public string OffImage { get; set; }
        public string ClickSound { get; set; }
        public Position Position { get; set; }
	}
}

