using Company.App.Application.UseCases.DetectBatman.Models;

namespace Company.App.Application.Interfaces
{
    public interface IHeroScanner
    {
        Task<IEnumerable<HeroDto>> ScanCsvAsync(byte[] fileData);
    }
}
