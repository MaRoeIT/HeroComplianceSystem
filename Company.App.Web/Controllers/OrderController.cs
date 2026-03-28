using Company.App.Application.Shared;
using Company.App.Application.UseCases.DataExtraction;
using Company.App.Application.UseCases.DataExtraction.Models;
using Company.App.Application.UseCases.DataMapping;
using Company.App.Application.UseCases.DataMapping.Models;
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

        [HttpPost("mapDocument")]
        public async Task<ActionResult<Result<DocumentMappingResultDto>>> MapDocument(IFormFile file)
        {
            _logger.LogInformation("MapDocument endpoint called");

            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            var command = new MapDocumentCommand(stream.ToArray());
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new
                {
                    message = "Document type could not be identified.",
                    DocumentType = result.Value?.DocumentType.ToString(),
                    suggestion = "Please controll that the uploaded content in the file corresponds to the intended dokument type."
                });

            return Ok(result);
        }
    }
}
