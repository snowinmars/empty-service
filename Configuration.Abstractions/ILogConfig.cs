using EmptyService.CommonEntities.Pathes;

namespace EmptyService.Configuration.Abstractions
{
    public interface ILogConfig
    {
        FilePath LogFilePath { get; }

        string Level { get; }
    }
}