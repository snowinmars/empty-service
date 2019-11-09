using System;
using System.Threading.Tasks;
using EmptyService.CommonEntities.Pathes;

namespace EmptyService.Logger.Abstractions
{
    public interface ILog
    {
        FilePath LogPath { get; }

        string LogLevel { get; }

        void CloseAndFlush();

        void Debug<T>(Exception exception, string messageTemplate, T propertyValue);

        void Debug(Exception exception, string messageTemplate);

        void Debug<T>(T item);

        void Debug(string message);

        void Error<T>(Exception exception, string messageTemplate, T propertyValue);

        void Error(Exception exception, string messageTemplate);

        void Error<T>(T item);

        void Error(string message);

        void Exception(Exception exception);

        void Exception(UnhandledExceptionEventArgs exceptionEventArgs);

        void Exception(UnobservedTaskExceptionEventArgs exception);

        void Fatal<T>(Exception exception, string messageTemplate, T propertyValue);

        void Fatal(Exception exception, string messageTemplate);

        void Fatal<T>(T item);

        void Fatal(string message);

        /// <summary>
        /// Call this function in ContinueWith method if you don't await an async method.
        /// </summary>
        /// <param name="task">_.</param>
        void FireForgetLog(Task task);

        void Information<T>(Exception exception, string messageTemplate, T propertyValue);

        void Information(Exception exception, string messageTemplate);

        void Information<T>(T item);

        void Information(string message);

        void Verbose<T>(Exception exception, string messageTemplate, T propertyValue);

        void Verbose(Exception exception, string messageTemplate);

        void Verbose<T>(T item);

        void Verbose(string message);

        void Warning<T>(Exception exception, string messageTemplate, T propertyValue);

        void Warning(Exception exception, string messageTemplate);

        void Warning<T>(T item);

        void Warning(string message);
    }
}