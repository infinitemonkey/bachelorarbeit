using System.Collections.Generic;

namespace Sidste.CrossFramework.Common.Configuration
{
	public class LevelDefinition
	{
        public string Key { get; set; }
        public Dictionary<string, string> Text { get; set; }
        public string DefaultImage { get; set; }
        public string ClickImage { get; set; }
        public string SuccessImage { get; set; }
        public string ClickSound { get; set; }
        public Position ScorePosition { get; set; }
        public string Content { get; set; }
	}
}

