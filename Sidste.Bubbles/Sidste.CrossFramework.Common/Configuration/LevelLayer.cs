using System.Collections.Generic;

namespace Sidste.CrossFramework.Common.Configuration
{
	public class LevelLayer
	{
        public string BackgroundImage { get; set; }
        public DefaultButton BackButton { get; set; }
        public Layout Layout { get; set; }
        public IList<LevelDefinition> Levels { get; set; }
	}
}

