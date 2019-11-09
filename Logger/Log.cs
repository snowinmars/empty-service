using System;
using System.Diagnostics;
using System.Threading.Tasks;
using EmptyService.CommonEntities.Pathes;
using EmptyService.Logger.Abstractions;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;

namespace EmptyService.Logger
{
    internal sealed class Log : ILog
    {
        public Log(FilePath logPath, string level)
        {
            var logLevel = (LogEventLevel)Enum.Parse(typeof(LogEventLevel), level);
            Console.WriteLine($"Log PID: {Process.GetCurrentProcess().Id}");

            LogPath = logPath;
            LogPath.TouchAsync().GetAwaiter().GetResult();

            log = new LoggerConfiguration()
                  .MinimumLevel.Is(logLevel)
                  .WriteTo.Console(LogEventLevel.Debug)
                  .WriteTo.File(LogPath, logLevel)
                  .CreateLogger();

            SelfLog.Enable(Console.WriteLine);

            LogLevel = logLevel.ToString();
        }

        public FilePath LogPath { get; }

        public string LogLevel { get; }

        private readonly Serilog.Core.Logger log;

        public void CloseAndFlush()
        {
            Serilog.Log.CloseAndFlush();
        }

        public void Debug<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            log.Debug(exception, messageTemplate, propertyValue);
        }

        public void Debug(Exception exception, string messageTemplate)
        {
            log.Debug(exception, messageTemplate);
        }

        public void Debug<T>(T item)
        {
            Debug(item.ToString());
        }

        public void Debug(string message)
        {
            log.Debug(message);
        }

        public void Error<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            log.Error(exception, messageTemplate, propertyValue);
        }

        public void Error(Exception exception, string messageTemplate)
        {
            log.Error(exception, messageTemplate);
        }

        public void Error<T>(T item)
        {
            Error(item.ToString());
        }

        public void Error(string message)
        {
            log.Error(message);
        }

        public void Exception(Exception exception)
        {
            log.Fatal(exception,
                      $"DEADBEAF: an exception was thrown. Current WindowsIdentity: {Environment.UserName}");
        }

        public void Exception(UnhandledExceptionEventArgs exceptionEventArgs)
        {
            log.Fatal((Exception)exceptionEventArgs.ExceptionObject, "DEADBEAF: an unhandled exception was thrown");
        }

        public void Exception(UnobservedTaskExceptionEventArgs exception)
        {
            log.Fatal(exception.Exception, "DEADBEAF: an unobserved exception was thrown");
        }

        public void Fatal<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            log.Fatal(exception, messageTemplate, propertyValue);
        }

        public void Fatal(Exception exception, string messageTemplate)
        {
            log.Fatal(exception, messageTemplate);
        }

        public void Fatal<T>(T item)
        {
            Fatal(item.ToString());
        }

        public void Fatal(string message)
        {
            log.Fatal(message);
        }

        /// <summary>
        /// Call this function in ContinueWith method if you don't await an async method.
        /// </summary>
        /// <param name="task">_.</param>
        public void FireForgetLog(Task task)
        {
            if (!(task.Exception is null))
            {
                var flatten = task.Exception.Flatten();
                Exception(flatten);

                throw task.Exception;
            }

            if (task.IsCanceled)
            {
                Warning("Task was cancelled without an exception");
            }

            if (task.IsFaulted)
            {
                Warning("Task was faulted without an exception");
            }
        }

        public void Information<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            log.Information(exception, messageTemplate, propertyValue);
        }

        public void Information(Exception exception, string messageTemplate)
        {
            log.Information(exception, messageTemplate);
        }

        public void Information<T>(T item)
        {
            Information(item.ToString());
        }

        public void Information(string message)
        {
            log.Information(message);
        }

        public void Verbose<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            log.Verbose(exception, messageTemplate, propertyValue);
        }

        public void Verbose(Exception exception, string messageTemplate)
        {
            log.Verbose(exception, messageTemplate);
        }

        public void Verbose<T>(T item)
        {
            Verbose(item.ToString());
        }

        public void Verbose(string message)
        {
            log.Verbose(message);
        }

        public void Warning<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            log.Warning(exception, messageTemplate, propertyValue);
        }

        public void Warning(Exception exception, string messageTemplate)
        {
            log.Warning(exception, messageTemplate);
        }

        public void Warning<T>(T item)
        {
            Warning(item.ToString());
        }

        public void Warning(string message)
        {
            log.Warning(message);
        }
    }
}