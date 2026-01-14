using domain;
using domain.Enums;

namespace application.Interfaces
{
    public interface ITradeAnalyzer
    {
        (List<RiskCategory> Categories, Dictionary<RiskCategory, TradeSummary> Summary) Analyze(List<Trade> trades);
    }
}
