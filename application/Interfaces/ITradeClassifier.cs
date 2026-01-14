using domain;
using domain.Enums;

namespace application.Interfaces
{
    public interface ITradeClassifier
    {
        RiskCategory Classify(Trade trade);
    }
}
