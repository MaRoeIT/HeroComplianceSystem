using Company.App.Application.Interfaces;
using Company.App.Application.Shared;
using Company.App.Domain.Specification;
using Company.App.Application.UseCases.DetectBatman.Models;

namespace Company.App.Application.UseCases.DetectBatman
{
    public class DetectBatmanUseCase
    {
        // Port
        private readonly IHeroScanner _scanner;

        public DetectBatmanUseCase(IHeroScanner scanner)
        {
            _scanner = scanner;
        }

        public async Task<Result<DetectionResult>> Execute(byte[] csvData)
        {
            IEnumerable<HeroDto> heroes = await _scanner.ScanCsvAsync(csvData);
            IsBatmanSpec spec = new IsBatmanSpec();

            foreach (HeroDto hero in heroes)
            {
                if(heroes == null) return new Result<DetectionResult>(null,false, "File was unreadable or empty");
                if(spec.IsBatman(hero.Name, hero.Identity))
                {
                    return new Result<DetectionResult>(IsSuccess:true,Value:new DetectionResult
                    (
                        Found:true,
                        Message: $"Batman detected at row {hero.RowNumber} with name '{hero.Name}' and secret identity '{hero.Identity}'.",
                        CheckedAt: DateTime.UtcNow
                    ));
                }
            }

            return new Result<DetectionResult>(IsSuccess:true,Value:new DetectionResult
            (
                Found: false,
                Message: "No Batman detected in the provided data.",
                CheckedAt: DateTime.UtcNow
            ));
        }
    }
}
