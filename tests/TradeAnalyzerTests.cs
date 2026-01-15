using application;
using domain;
using domain.Enums;
using tests.Mocks;

namespace tests
{
    public class TradeAnalyzerTests
    {
        [Fact]
        public void Analyze_MultipleTrades_ReturnsExpectedCategoriesAndSummary()
        {
            var mapping = new Dictionary<string, RiskCategory>
            {
                ["ClientA"] = RiskCategory.LOWRISK,
                ["ClientB"] = RiskCategory.HIGHRISK
            };
            var classifier = new FakeClassifier(mapping);
            var analyzer = new TradeAnalyzer(classifier);

            var trades = new List<Trade>
            {
                new Trade { Value = 500m, ClientSector = ClientSector.Private, ClientId = "ClientA" },
                new Trade { Value = 2_000m, ClientSector = ClientSector.Public,  ClientId = "ClientB" },
                new Trade { Value = 3_000m, ClientSector = ClientSector.Private, ClientId = "ClientA" }
            };

            var (categories, summary, processingTimeMs) = analyzer.Analyze(trades);

            Assert.Equal(new List<RiskCategory> { RiskCategory.LOWRISK, RiskCategory.HIGHRISK, RiskCategory.LOWRISK }, categories);

            Assert.True(summary.ContainsKey(RiskCategory.LOWRISK));
            Assert.True(summary.ContainsKey(RiskCategory.HIGHRISK));

            var low = summary[RiskCategory.LOWRISK];
            Assert.Equal(2, low.Count);
            Assert.Equal(3_500m, low.TotalValue);
            Assert.Equal("ClientA", low.TopClient);

            var high = summary[RiskCategory.HIGHRISK];
            Assert.Equal(1, high.Count);
            Assert.Equal(2_000m, high.TotalValue);
            Assert.Equal("ClientB", high.TopClient);

            Assert.Equal(trades.Count, classifier.CallCount);
            Assert.True(processingTimeMs >= 0);
        }

        [Fact]
        public void Analyze_EmptyList_ReturnsEmptyResults()
        {
            var classifier = new FakeClassifier(new Dictionary<string, RiskCategory>());
            var analyzer = new TradeAnalyzer(classifier);

            var trades = new List<Trade>();

            var (categories, summary, processingTimeMs) = analyzer.Analyze(trades);

            Assert.Empty(categories);
            Assert.Empty(summary);
            Assert.Equal(0, classifier.CallCount);
            Assert.True(processingTimeMs >= 0);
        }
    }
}