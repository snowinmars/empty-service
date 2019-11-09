using Newtonsoft.Json;

namespace EmptyService.DependencyResolver.ConfigurationModels
{
    internal sealed class DatabaseConfigModel
    {
        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("port")]
        public string Port { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("database-name")]
        public string DatabaseName { get; set; }
    }
}