using application.Interfaces;
using domain;
using domain.Enums;

namespace application
{
    public class TradeClassifier : ITradeClassifier
    {
        public RiskCategory Classify(Trade trade)
        {
            if (trade.Value < 1000000)
                return RiskCategory.LOWRISK;
            return trade.ClientSector == ClientSector.Public
                ? RiskCategory.MEDIUMRISK
                : RiskCategory.HIGHRISK;
        }
    }
}
