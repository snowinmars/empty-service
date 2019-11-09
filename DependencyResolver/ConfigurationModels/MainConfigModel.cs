using Newtonsoft.Json;

namespace EmptyService.DependencyResolver.ConfigurationModels
{
    internal sealed class MainConfigModel
    {
        [JsonProperty("dbUsageName")]
        public DatabaseConfigModel MyDatabase { get; set; }

        [JsonProperty("log")]
        public LogConfigModel Log { get; set; }
    }
}