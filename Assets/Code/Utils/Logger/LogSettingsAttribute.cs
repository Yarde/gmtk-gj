using System;

namespace Yarde.Utils.Logger 
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
	public class LogSettingsAttribute : Attribute 
	{
		public string Tag { get; set; }

		public string Color { get; }

		public LogSettingsAttribute(string tag = null, string color = null) {
			Tag = tag;
			Color = color;
		}
	}
}