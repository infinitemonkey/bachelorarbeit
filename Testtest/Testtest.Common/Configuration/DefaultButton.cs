using System.Collections.Generic;

namespace Testtest.Common.Configuration
{
	public class DefaultButton
	{
        public Dictionary<string, string> Text { get; set; }
        public string DefaultImage { get; set; }
        public string ClickImage { get; set; }
        public string ClickSound { get; set; }
        public Position Position { get; set; } 
	}
}

