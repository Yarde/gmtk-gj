using System;
using System.Linq;
using JetBrains.Annotations;

namespace Yarde.Utils.Logger
{

    public static class Logger
    {
        [PublicAPI] public static ILogger ActiveLogger { private get; set; }

        [PublicAPI] public static LoggerLevel Level { get; set; }

        static Logger()
        {
#if UNITY_EDITOR
            ActiveLogger = new EditorLogger();
#else
			ActiveLogger = new RuntimeLogger();
#endif
            Level = LoggerLevel.Verbose;
        }

        [PublicAPI]
        public static void Log(this object caller, string message)
        {
            LogInfo(caller, message);
        }

        [PublicAPI]
        public static void LogVerbose(this object caller, string message)
        {
            Log(caller, LoggerLevel.Verbose, message);
        }

        [PublicAPI]
        public static void LogInfo(this object caller, string message)
        {
            Log(caller, LoggerLevel.Info, message);
        }

        [PublicAPI]
        public static void LogWarning(this object caller, string message)
        {
            Log(caller, LoggerLevel.Warning, message);
        }

        [PublicAPI]
        public static void LogError(this object caller, string message)
        {
            Log(caller, LoggerLevel.Error, message);
        }

        [PublicAPI]
        public static void Log(this object caller, LoggerLevel level, string message)
        {
            if (level < Level) return;

            var settings = GetSettingsFromCaller(caller);
            ActiveLogger.Log(level, settings, message);
        }

        [NotNull]
        private static LogSettingsAttribute GetSettingsFromCaller(object caller)
        {
            Type type;
            if (caller is Type callerType)
            {
                type = callerType;
            }
            else
            {
                type = caller.GetType();
            }

            var attributeType = typeof(LogSettingsAttribute);
            var attribute = type.GetCustomAttributes(attributeType, true)
                .Union(type.GetInterfaces().SelectMany(interfaceType =>
                    interfaceType.GetCustomAttributes(attributeType, true)))
                .FirstOrDefault() as LogSettingsAttribute;
            if (attribute is { Tag: null })
            {
                attribute.Tag = caller.GetType().Name;
            }
            return attribute ?? new LogSettingsAttribute(caller.GetType().Name);
        }
    }
}
