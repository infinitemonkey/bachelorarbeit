using System.Collections.Generic;

namespace Testtest.Common.Configuration
{
	public class LevelLayer
	{
        public string BackgroundImage { get; set; }
        public DefaultButton BackButton { get; set; }
        public Layout Layout { get; set; }
        public IEnumerable<LevelDefinition> Levels { get; set; }
	}
}

