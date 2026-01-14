using Microsoft.AspNetCore.Mvc;
using domain;
using application.Interfaces;

namespace trade_classification_api.Controllers
{
    [ApiController]
    [Route("api/trades/")]
    public class TradesController : ControllerBase
    {
        private readonly ILogger<TradesController> _logger;
        private readonly ITradeClassifier _classifier;
        private readonly ITradeAnalyzer _analyzer;

        public TradesController(ILogger<TradesController> logger, ITradeClassifier classifier, ITradeAnalyzer analyzer)
        {
            _logger = logger;
            _classifier = classifier;
            _analyzer = analyzer;
        }

        [HttpPost("classify")]
        public IActionResult Classify([FromBody] List<Trade> trades)
        {
            if (trades == null || !trades.Any()) return BadRequest("Invalid input");

            try
            {
                _logger.LogInformation("Classifying {Count} trades", trades.Count);
                var categories = trades.Select(t => _classifier.Classify(t)).ToList();
                return Ok(new { categories });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error classifying trades");
                return StatusCode(500, "Internal error");
            }
        }

        [HttpPost("analyze")]
        public IActionResult Analyze([FromBody] List<Trade> trades)
        {
            if (trades == null || !trades.Any()) return BadRequest("Invalid input");
            try
            {
                _logger.LogInformation("Analyzing {Count} trades", trades.Count);
                var (categories, summary) = _analyzer.Analyze(trades);
                return Ok(new { categories, summary });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error analyzing trades");
                return StatusCode(500, "Internal error");
            }
        }
    }
}
