using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Yarde.Utils.Logger {
    [PublicAPI]
    internal class RuntimeLogger : ILogger {
        public void Log(LoggerLevel level, LogSettingsAttribute settings, string message) {
            var composedMessage = !string.IsNullOrWhiteSpace(settings.Tag) ? $"[{settings.Tag}] → {message}" : message;

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
