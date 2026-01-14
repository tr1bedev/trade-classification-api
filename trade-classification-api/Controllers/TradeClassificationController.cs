using Microsoft.AspNetCore.Mvc;
using trade_classification_api.Models;
using trade_classification_api.Models.Enums;

namespace trade_classification_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TradeClassificationController : ControllerBase
    {
        private readonly ILogger<TradeClassificationController> _logger;

        public TradeClassificationController(ILogger<TradeClassificationController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "GetClassification")]
        public TradeClassification GetClassification(Trade trade)
        {
            return new TradeClassification
            {
                Categories = new List<TradeClassificationEnum>
                {
                    TradeClassificationEnum.HighRisk,
                    TradeClassificationEnum.LowRisk,
                    TradeClassificationEnum.MediumRisk
                }
            };
        }
    }
}
