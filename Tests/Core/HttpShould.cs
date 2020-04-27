using NUnit.Framework;
using Onbox.Core.V2;
using Onbox.Core.V2.Http;
using Onbox.Core.V2.Json;
using Onbox.Di.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Onbox.Core.V2.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core
{
    [TestFixture]
    public class HttpShould
    {
        HttpService sut;
        string token;

        [OneTimeSetUp]
        public void Setup()
        {
            var mockJson = new JsonService(new JsonSerializerSettings());
            var mockLogging = new FileLoggingService(new FileLoggingSettings());
            var mockHttpSettings = new HttpSettings();

            sut = new HttpService(mockJson, mockLogging, mockHttpSettings);
        }

        [SetUp]
        public async Task SetupToken()
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                var endpoint = "https://developer.api.autodesk.com/authentication/v1/authenticate";

                var payload = new Dictionary<string, string>();
                payload.Add("client_id", "zZjEYAnZyoYsExyqt1HsrPmUSA33kQtr");
                payload.Add("client_secret", "9zqHQEY1hofKfvGi");
                payload.Add("grant_type", "client_credentials");
                payload.Add("scope", "data:search data:read bucket:read bucket:create data:write bucket:delete account:read account:write");

                var tokenPayload = await this.sut.PostFormAsync<JObject>(endpoint, payload);
                this.token = tokenPayload["access_token"].ToString();
            }
        }

        [Test]
        public async Task GetJson()
        {
            var endpoint = "https://samples.openweathermap.org/data/2.5/weather?q=London,uk&appid=b6907d289e10d714a6e88b30761fae22";
            var result = await this.sut.GetAsync<JObject>(endpoint);

            Assert.That(result["coord"]["lon"].ToString(), Is.EqualTo("-0.13"));
            Assert.That(result["coord"]["lat"].ToString(), Is.EqualTo("51.51"));
        }

        [Test]
        public async Task GetJsonWithToken()
        {
            var accountId = "660ae58d-8049-45ca-b1ad-60858d4d934b";
            var endpoint = $"https://developer.api.autodesk.com/hq/v1/accounts/{accountId}/projects";
            var result = await this.sut.GetAsync<JArray>(endpoint, token);

            Assert.That(result[0]["id"].ToString(), Is.Not.Null);
        }

        [Test]
        public async Task GetStream()
        {
            var endpoint = "https://www.google.com/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png";
            using (var stream = await this.sut.GetStreamAsync(endpoint))
            {
                Assert.That(stream.CanRead);
                Assert.That(stream.Length > 0);
            }
        }

    }
}
