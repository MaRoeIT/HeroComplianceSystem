using Company.App.Application.Shared;
using MediatR;

namespace Company.App.Application.UseCases.DetectBatman
{
    public record DetectBatmanCommand(byte[] FileData) : IRequest<Result<DetectionResult>>;
}
