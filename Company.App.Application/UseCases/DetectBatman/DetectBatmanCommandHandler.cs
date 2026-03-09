using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DetectBatman.Models;
using Company.App.Application.Shared;
using Company.App.Domain.Specification;
using MediatR;

namespace Company.App.Application.UseCases.DetectBatman
{
    public class DetectBatmanCommandHandler : IRequestHandler<DetectBatmanCommand, Result<DetectionResult>>
    {
        private readonly IHeroScanner _scanner;

        public DetectBatmanCommandHandler(IHeroScanner scanner)
        {
            _scanner = scanner;
        }

        public async Task<Result<DetectionResult>> Handle(DetectBatmanCommand request, CancellationToken cancellationToken)
        {
            var heroes = await _scanner.ScanCsvAsync(request.FileData);

            var spec = new IsBatmanSpec();

            if(heroes == null || !heroes.Any())
            {
                return new Result<DetectionResult>(null, false, "File was unreadable or empty");
            }

            foreach (var hero in heroes)
            {
                if (spec.IsBatman(hero.Name, hero.Identity))
                {
                    return new Result<DetectionResult>(IsSuccess: true, Value: new DetectionResult
                    (
                        Found: true,
                        Message: $"Batman detected at row {hero.RowNumber} with name '{hero.Name}' and secret identity '{hero.Identity}'.",
                        CheckedAt: DateTime.UtcNow
                    ));
                }
            }

            return new Result<DetectionResult>(IsSuccess: true, Value: new DetectionResult
            (
                Found: false,
                Message: "No Batman detected in the provided data.",
                CheckedAt: DateTime.UtcNow
            ));
        }
    }
}
