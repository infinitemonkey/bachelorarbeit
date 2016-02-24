using System;
using System.Collections.Generic;

namespace Testtest.Common
{
	public class DefaultButton
	{
        public IEnumerable<Dictionary<string, string>> Text { get; set; }
        public string DefaultImage { get; set; }
        public string ClickImage { get; set; }
        public string ClickSound { get; set; }
        public Position Position { get; set; } 
	}
}

