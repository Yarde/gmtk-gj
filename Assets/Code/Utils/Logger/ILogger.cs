namespace Yarde.Utils.Logger { 
	public interface ILogger 
	{ 
		void Log(LoggerLevel level, LogSettingsAttribute settings, string message);
	}
}