using application;
using domain;
using domain.Enums;

namespace tests
{
    public class TradeClassifierTests
    {
        [Fact]
        public void Classify_LowValue_ReturnsLowRisk()
        {
            var classifier = new TradeClassifier();
            var trade = new Trade { Value = 999_999m, ClientSector = ClientSector.Private };
            Assert.Equal(RiskCategory.LOWRISK, classifier.Classify(trade));
        }

        [Fact]
        public void Classify_ExactlyOneMillion_Public_ReturnsMediumRisk()
        {
            var classifier = new TradeClassifier();
            var trade = new Trade { Value = 1_000_000m, ClientSector = ClientSector.Public };
            Assert.Equal(RiskCategory.MEDIUMRISK, classifier.Classify(trade));
        }

        [Fact]
        public void Classify_ExactlyOneMillion_Private_ReturnsHighRisk()
        {
            var classifier = new TradeClassifier();
            var trade = new Trade { Value = 1_000_000m, ClientSector = ClientSector.Private };
            Assert.Equal(RiskCategory.HIGHRISK, classifier.Classify(trade));
        }

        [Fact]
        public void Classify_InvalidSector_ReturnsHighRisk()
        {
            var classifier = new TradeClassifier();
            var trade = new Trade { Value = 2_000_000m, ClientSector = ClientSector.Unknown };
            Assert.Equal(RiskCategory.HIGHRISK, classifier.Classify(trade));
        }
    }
}