using Company.App.Application.Shared;
using Company.App.Application.UseCases.AddHero;
using Company.App.Application.UseCases.DetectBatman;
using Company.App.Application.UseCases.DetectBatman.Models;
using Company.App.Application.UseCases.LookupHeroByName;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Company.App.Web.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class HeroController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<HeroController> _logger;

        public HeroController(IMediator mediator, ILogger<HeroController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("detectBatman")]
        public async Task<ActionResult<Result<DetectionResult>>> UploadCsv(IFormFile file)
        {
            _logger.LogInformation("DetectBatman endpoint called");

            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("No file uploaded");
                return BadRequest("No file uploaded.");
            }

            _logger.LogInformation($"Processing file: {file.FileName}, Size: {file.Length}");

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            var result = await _mediator.Send(new DetectBatmanCommand(stream.ToArray()));

            _logger.LogInformation($"Detection complete: {result.IsSuccess}");
            return Ok(result);
        }

        [HttpGet("heroes")]
        public async Task<ActionResult<Result<List<HeroByNameResult>>>> GetAllHeroes()
        {
            throw new NotImplementedException();
        }

        [HttpGet("heroes/{name}")]
        public async Task<ActionResult<Result<HeroByNameResult>>> GetHeroByName(string name)
        {
            _logger.LogInformation("Heroes/{name} endpoint called");

            if(string.IsNullOrEmpty(name))
            {
                _logger.LogInformation($"Name is null or empty");
                return BadRequest("Name is null or empty");
            }

            var result = await _mediator.Send(new LookupHeroByNameQuery(name));
            return Ok(result);
        }

        [HttpPost("heroes")]
        public async Task<ActionResult<Result>> AddHero(HeroDto hero)
        {
            if(hero == null)
            {
                return BadRequest("Hero is not included");
            }

            var result = await _mediator.Send(new AddHeroCommand(hero));
            return Ok(result);
        }
    }
}
