using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Esys.Thingsboard.Clients.Tests
{
    public class HttpClientTests : ConfigurableTests
    {
        protected readonly HttpClient httpClient;

        public HttpClientTests()
        {
            httpClient = new HttpClient { BaseAddress = new Uri(configuration["BaseUrl"]) };
        }


    }
}
