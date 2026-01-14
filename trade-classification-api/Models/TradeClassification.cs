using trade_classification_api.Models.Enums;

namespace trade_classification_api.Models
{
    public class TradeClassification
    {
        public List<TradeClassificationEnum> Categories { get; set; } = [];
    }
}
