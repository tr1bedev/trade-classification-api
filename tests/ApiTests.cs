using domain;
using domain.Enums;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;
using System.Text.Json;

namespace tests
{
    public class ApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ApiTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Classify_ReturnsCategories()
        {
            var trades = new List<Trade>
            {
                new Trade { Value = 500m, ClientSector = ClientSector.Private, ClientId = "ClientA" },       
                new Trade { Value = 1_000_000m, ClientSector = ClientSector.Public,  ClientId = "ClientB" },  
                new Trade { Value = 2_000_000m, ClientSector = ClientSector.Private, ClientId = "ClientC" }   
            };

            var response = await _client.PostAsJsonAsync("/api/trades/classify", trades);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            Assert.True(json.TryGetProperty("categories", out var categoriesElement));
            Assert.Equal(3, categoriesElement.GetArrayLength());

            Assert.Equal("LOWRISK", categoriesElement[0].GetString());
            Assert.Equal("MEDIUMRISK", categoriesElement[1].GetString());
            Assert.Equal("HIGHRISK", categoriesElement[2].GetString());
        }

        [Fact]
        public async Task Analyze_ReturnsSummaryAndProcessingTime()
        {
            var trades = new List<Trade>
            {
                new Trade { Value = 500m, ClientSector = ClientSector.Private, ClientId = "ClientA" },
                new Trade { Value = 2_000_000m, ClientSector = ClientSector.Private, ClientId = "ClientB" },
                new Trade { Value = 3_000m, ClientSector = ClientSector.Private, ClientId = "ClientA" }
            };

            var response = await _client.PostAsJsonAsync("/api/trades/analyze", trades);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();

            Assert.True(json.TryGetProperty("categories", out var categoriesElement));
            Assert.Equal(3, categoriesElement.GetArrayLength());
            
            Assert.Equal("LOWRISK", categoriesElement[0].GetString());
            Assert.Equal("HIGHRISK", categoriesElement[1].GetString());
            Assert.Equal("LOWRISK", categoriesElement[2].GetString());

            Assert.True(json.TryGetProperty("summary", out var summaryElement));
            Assert.True(summaryElement.TryGetProperty("LOWRISK", out var lowElement));
            Assert.True(summaryElement.TryGetProperty("HIGHRISK", out var highElement));

            Assert.Equal(2, lowElement.GetProperty("count").GetInt32());
            Assert.Equal(3_500m, lowElement.GetProperty("totalValue").GetDecimal());
            Assert.Equal("ClientA", lowElement.GetProperty("topClient").GetString());

            Assert.Equal(1, highElement.GetProperty("count").GetInt32());
            Assert.Equal(2_000_000m, highElement.GetProperty("totalValue").GetDecimal());
            Assert.Equal("ClientB", highElement.GetProperty("topClient").GetString());

            Assert.True(json.TryGetProperty("processingTimeMs", out var procElement));
            Assert.True(procElement.GetInt64() >= 0);
        }
    }
}
