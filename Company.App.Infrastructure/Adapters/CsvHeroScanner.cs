using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DetectBatman.Models;
using CsvHelper.Configuration;
using System.Globalization;

namespace Company.App.Infrastructure.Adapters
{
    public class CsvHeroScanner: IHeroScanner
    {
        public async Task<IEnumerable<HeroDto>> ScanCsvAsync(byte[] fileData)
        {
            using var reader = new StreamReader(new MemoryStream(fileData));

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                PrepareHeaderForMatch = args => args.Header.ToLower(),
            };

            using var csv = new CsvHelper.CsvReader(reader, config);

            List<HeroDto> heroes = new List<HeroDto>();
            int row = 1;

            await foreach (var record in csv.GetRecordsAsync<dynamic>())
            {
                row++;
                heroes.Add(new HeroDto(
                    Name: record.name ?? "Unknown",
                    Identity: record.identity ?? "Unknown",
                    RowNumber: row
                ));
                
            }

            return heroes;
        }
    }
}
