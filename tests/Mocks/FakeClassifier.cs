using application.Interfaces;
using domain;
using domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tests.Mocks
{
    public class FakeClassifier : ITradeClassifier
    {
        private readonly Dictionary<string, RiskCategory> _byClient;
        public int CallCount { get; private set; }

        public FakeClassifier(Dictionary<string, RiskCategory> byClient)
        {
            _byClient = byClient;
        }

        public RiskCategory Classify(Trade trade)
        {
            CallCount++;
            return _byClient.TryGetValue(trade.ClientId, out var cat) ? cat : RiskCategory.HIGHRISK;
        }
    }
}
