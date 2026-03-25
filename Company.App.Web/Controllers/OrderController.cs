using Company.App.Application.Shared;
using Company.App.Application.UseCases.DataExtraction;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.UseCases.DetectBatman;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Company.App.Web.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IMediator mediator, ILogger<OrderController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("extractPdf")]
        public async Task<ActionResult<Result<ExtractedDocumentDto>>> ExtractPdf(IFormFile file)
        {
            _logger.LogInformation("DataExtraction endpoint called");

            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("No file uploaded");
                return BadRequest("No file uploaded.");
            }

            _logger.LogInformation($"Processing file: {file.FileName}, Size: {file.Length}");

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            var result = await _mediator.Send(new ExtractPdfCommand(stream.ToArray()));

            return Ok(result);
        }
    }
}
