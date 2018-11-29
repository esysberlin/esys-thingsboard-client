using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Esys.Thingsboard.Clients.Tests
{
    public class AuthTests
    {
        static readonly IConfiguration configuration;

        static AuthTests()
        {
            configuration = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .AddJsonFile("config.local.json", optional: true)
                .Build();
        }

        readonly HttpClient httpClient;

        public AuthTests()
        {
            httpClient = new HttpClient { BaseAddress = new Uri(configuration["BaseUrl"]) };
        }

        [Theory]
        [InlineData("eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiJzeXNhZG1pbkB0aGluZ3Nib2FyZC5vcmciLCJzY29wZXMiOlsiU1lTX0FETUlOIl0sInVzZXJJZCI6Ijg3YmUzYTUwLWQ3OTUtMTFlOC1hZDFkLTcxODk4YjIyY2I3NCIsImVuYWJsZWQiOnRydWUsImlzUHVibGljIjpmYWxzZSwidGVuYW50SWQiOiIxMzgxNDAwMC0xZGQyLTExYjItODA4MC04MDgwODA4MDgwODAiLCJjdXN0b21lcklkIjoiMTM4MTQwMDAtMWRkMi0xMWIyLTgwODAtODA4MDgwODA4MDgwIiwiaXNzIjoidGhpbmdzYm9hcmQuaW8iLCJpYXQiOjE1NDM0MDkyMjAsImV4cCI6MTU0MzQxMDEyMH0.TM4uP5WbDP4HNfIKAX3wehUMReHPYhnilcJw8HItY4sQbUbWSR-We8-VFrrj217gSXLDiYfH7OGAnPG8Bl0IJw", "thingsboard.io", "sysadmin@thingsboard.org")]
        private static void JwtTokenTest(string token, string issuer, string subject)
        {
            Assert.NotNull(token);
            Assert.NotEmpty(token);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            Assert.True(jwtSecurityTokenHandler.CanReadToken(token));
            var jwt = jwtSecurityTokenHandler.ReadJwtToken(token);
            Assert.NotNull(jwt);
            Assert.Equal(issuer, jwt.Issuer);
            Assert.Equal(subject, jwt.Subject);
        }

        public static IEnumerable<object[]> LoginTestData => new List<object[]>
        {
            new object[] { configuration["Username"], configuration["Password"] },
        };

        [Theory]
        [MemberData(nameof(LoginTestData))]
        public async Task LoginTest(string username, string password)
        {
            var client = new AuthClient(httpClient);
            var loginResponse = await client.LoginAsync(username, password);
            Assert.NotNull(loginResponse);
            JwtTokenTest(loginResponse.Token, "thingsboard.io", username);
            JwtTokenTest(loginResponse.RefreshToken, "thingsboard.io", username);
        }

        public static IEnumerable<object[]> GetUserTestData => new List<object[]>
        {
            new object[] { configuration["Username"], configuration["Password"] },
        };

        [Theory]
        [MemberData(nameof(GetUserTestData))]
        public async Task GetUserTest(string username, string password)
        {
            var client = new AuthClient(httpClient);
            var loginResponse = await client.LoginAsync(username, password);
            httpClient.DefaultRequestHeaders.Add("X-Authorization", $"Bearer {loginResponse.Token}");
            var getUserResponse = await client.UserAsync();
            Assert.NotNull(getUserResponse);
            Assert.Equal(username, getUserResponse.Name);
        }
    }
}
