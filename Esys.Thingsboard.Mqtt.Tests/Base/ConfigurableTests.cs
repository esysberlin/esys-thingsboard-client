using Microsoft.Extensions.Configuration;

namespace Esys.Thingsboard.Mqtt.Tests
{
    public class ConfigurableTests
    {
        protected static readonly IConfiguration configuration;

        static ConfigurableTests() => configuration = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .AddJsonFile("config.local.json", optional: true)
                .Build();
    }
}
