using application.Interfaces;
using domain;
using domain.Enums;
using System.Diagnostics;

namespace application
{
    public class TradeAnalyzer : ITradeAnalyzer
    {
        private readonly ITradeClassifier _classifier;

        public TradeAnalyzer(ITradeClassifier classifier)
        {
            _classifier = classifier;
        }

        public (List<RiskCategory> Categories, Dictionary<RiskCategory, TradeSummary> Summary) Analyze(List<Trade> trades)
        {
            var stopwatch = Stopwatch.StartNew();
            var categories = new List<RiskCategory>(trades.Count);
            var summaries = new Dictionary<RiskCategory, (int Count, decimal TotalValue, Dictionary<string, decimal> ClientExposures)>();

            foreach (var trade in trades)
            {
                var category = _classifier.Classify(trade);
                categories.Add(category);

                if (!summaries.TryGetValue(category, out var data))
                {
                    data = (0, 0m, new Dictionary<string, decimal>());
                    summaries[category] = data;
                }

                data.Count++;
                data.TotalValue += trade.Value;
                data.ClientExposures.TryGetValue(trade.ClientId, out var exposure);
                data.ClientExposures[trade.ClientId] = exposure + trade.Value;
            }

            var finalSummary = new Dictionary<RiskCategory, TradeSummary>();
            foreach (var kvp in summaries)
            {
                var topClient = kvp.Value.ClientExposures.OrderByDescending(x => x.Value).FirstOrDefault().Key ?? "N/A";
                finalSummary[kvp.Key] = new TradeSummary
                {
                    Count = kvp.Value.Count,
                    TotalValue = kvp.Value.TotalValue,
                    TopClient = topClient
                };
            }

            stopwatch.Stop();
            return (categories, finalSummary);
        }
    }
}
