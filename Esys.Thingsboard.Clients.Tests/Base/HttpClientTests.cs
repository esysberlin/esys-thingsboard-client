using System;
using System.Net.Http;

namespace Esys.Thingsboard.Clients.Tests
{
    public class HttpClientTests : ConfigurableTests
    {
        protected readonly HttpClient httpClient;

        public HttpClientTests() => httpClient = new HttpClient { BaseAddress = new Uri(configuration["BaseUrl"]) };


    }
}
