using Company.App.Application.UseCases.DetectBatman.Models;
namespace Company.App.Application.Interfaces
{
    public interface ICheckRecords
    {
        Task<IEnumerable<CheckRecordDto>> readCsvAsync();
    }
}
