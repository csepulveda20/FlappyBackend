using Application.UseCase;
using Application.UseCase.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Presentation.Hubs;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/scores")]
    public class ScoreController(ScoreUseCase scoreUseCase, IHubContext<RankingHub> hubContext) : ControllerBase
    {
        private readonly ScoreUseCase _scoreUseCase = scoreUseCase;
        private readonly IHubContext<RankingHub> _hubContext = hubContext;

        // POST /api/v1/scores
        [HttpPost]
        public async Task<IActionResult> RegisterScore([FromBody] ScoreDto scoreDto)
        {
            if (string.IsNullOrWhiteSpace(scoreDto.Alias) || scoreDto.Points < 0)
                return BadRequest("Alias is required and Points must be >= 0.");

            var result = await _scoreUseCase.RegisterScoreAsync(scoreDto);

            var topScores = await _scoreUseCase.GetTopScoresAsync(5);

            await _hubContext.Clients.All.SendAsync("ScoreUpdated", topScores);

            return Ok(result);
        }

        // GET /api/v1/scores/top?limit=10
        [HttpGet("top")]
        public async Task<IActionResult> GetTopScores([FromQuery] int limit = 10)
        {
            var result = await _scoreUseCase.GetTopScoresAsync(limit);
            return Ok(result);
        }

        // GET /api/v1/scores/alias/{alias}
        [HttpGet("alias/{alias}")]
        public async Task<IActionResult> GetScoresByAlias(string alias)
        {
            if (string.IsNullOrWhiteSpace(alias))
                return BadRequest("Alias is required.");
            var result = await _scoreUseCase.GetScoresByAliasAsync(alias);
            return Ok(result);
        }
    }
}
