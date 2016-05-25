using System.Collections.Generic;

namespace Sidste.CrossFramework.Common.Configuration
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

