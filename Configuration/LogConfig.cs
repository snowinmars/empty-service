using EmptyService.CommonEntities.Pathes;
using EmptyService.Configuration.Abstractions;

namespace EmptyService.Configuration
{
    internal sealed class LogConfig : ILogConfig
    {
        public LogConfig(FilePath logFilePath, string level)
        {
            LogFilePath = logFilePath;
            Level = level;
        }

        public FilePath LogFilePath { get; }

        public string Level { get; }
    }
}