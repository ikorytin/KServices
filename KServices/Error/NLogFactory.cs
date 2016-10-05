﻿using System;
using System.Diagnostics;
using Microsoft.Owin.Logging;
using NLog;

namespace KServices.Error
{
    public class NLogFactory : ILoggerFactory
    {
        /// <summary>
        /// The log level translation function to get a NLog loglevel
        /// </summary>
        readonly Func<TraceEventType, LogLevel> _getLogLevel;

        /// <summary>
        /// Create a logger factory with the default translation
        /// </summary>
        public NLogFactory()
        {
            _getLogLevel = DefaultGetLogLevel;
        }

        /// <summary>
        /// Create a logger factory with a custom translation routine
        /// </summary>
        /// <param name="getLogLevel"></param>
        public NLogFactory(Func<TraceEventType, LogLevel> getLogLevel)
        {
            this._getLogLevel = getLogLevel;
        }

        /// <summary>
        /// This is the standard translation
        /// </summary>
        /// <param name="traceEventType"></param>
        /// <returns></returns>
        static LogLevel DefaultGetLogLevel(TraceEventType traceEventType)
        {
            switch (traceEventType)
            {
                case TraceEventType.Critical:
                    return LogLevel.Fatal;
                case TraceEventType.Error:
                    return LogLevel.Error;
                case TraceEventType.Warning:
                    return LogLevel.Warn;
                case TraceEventType.Information:
                    return LogLevel.Info;
                case TraceEventType.Verbose:
                    return LogLevel.Trace;
                case TraceEventType.Start:
                    return LogLevel.Debug;
                case TraceEventType.Stop:
                    return LogLevel.Debug;
                case TraceEventType.Suspend:
                    return LogLevel.Debug;
                case TraceEventType.Resume:
                    return LogLevel.Debug;
                case TraceEventType.Transfer:
                    return LogLevel.Debug;
                default:
                    throw new ArgumentOutOfRangeException("traceEventType");
            }
        }

        /// <summary>
        /// Creates a new ILogger instance of the given name.
        /// </summary>
        /// <param name="name">The logger context name.</param>
        /// <returns>A logger instance.</returns>
        public Microsoft.Owin.Logging.ILogger Create(string name)
        {
            return new Logger(name, _getLogLevel);
        }

        /// <summary>
        /// The wrapper arround NLog. Translates the logging levels
        /// </summary>
        class Logger : Microsoft.Owin.Logging.ILogger
        {
            readonly Func<TraceEventType, LogLevel> getLogLevel;

            readonly NLog.Logger logger;

            internal Logger(string name, Func<TraceEventType, LogLevel> getLogLevel)
            {
                this.getLogLevel = getLogLevel;
                logger = LogManager.GetLogger(name);
            }

            public bool WriteCore(TraceEventType eventType, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
            {
                var level = getLogLevel(eventType);

                // According to docs http://katanaproject.codeplex.com/SourceControl/latest#src/Microsoft.Owin/Logging/ILogger.cs
                // "To check IsEnabled call WriteCore with only TraceEventType and check the return value, no event will be written."
                if (state == null)
                {
                    return logger.IsEnabled(level);
                }

                if (!logger.IsEnabled(level))
                {
                    return false;
                }

                logger.Log(level, formatter(state, exception), exception);
                return true;
            }
        }
    }
}