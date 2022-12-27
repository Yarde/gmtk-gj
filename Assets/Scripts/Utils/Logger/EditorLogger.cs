using System;
using UnityEngine;

namespace Yarde.Utils.Logger 
{
    internal class EditorLogger : ILogger 
    {
        public void Log(LoggerLevel level, LogSettingsAttribute settings, string message) {
            string composedTag;
            if (!string.IsNullOrWhiteSpace(settings.Tag)) {
                composedTag = !string.IsNullOrEmpty(settings.Color) ?
                    $"[<color={settings.Color}><b>{settings.Tag}</b></color>] → " :
                    $"[<b>{settings.Tag}</b>] → ";
            } else {
                composedTag = string.Empty;
            }

            var composedMessage = $"{composedTag}{message}";
				
            switch (level) {
                case LoggerLevel.Verbose:
                    Debug.Log(composedMessage);
                    break;
                case LoggerLevel.Info:
                    Debug.Log(composedMessage);
                    break;
                case LoggerLevel.Warning:
                    Debug.LogWarning(composedMessage);
                    break;
                case LoggerLevel.Error:
                    Debug.LogError(composedMessage);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }
    }
}
