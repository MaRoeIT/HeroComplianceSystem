namespace Company.App.Infrastructure.Adapters;
using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DetectBatman.Models;
using CsvHelper.Configuration;

public class CsvHistorySave
	{
		private readonly string _filePath;
		public CsvHistorySave(string filePath)
		{
			_filePath = filePath;
		}
		public async Task SaveHistoryAsync(string stuff)
		{
			using var writer = new StreamWriter(_filePath, append: true);
			await writer.WriteLineAsync(stuff);
		}
	}
	public class CsvHistoryReader: ICheckRecords {
	public async Task<IEnumerable<CheckRecordDto>> readCsvAsync() 
	{
		List<CheckRecordDto> records = new List<CheckRecordDto>();


		return records; 
	}
		private readonly string _filePath;
		public CsvHistoryReader(string filePath)
		{
			_filePath = filePath;
		}
		public async Task<IEnumerable<string>> ReadHistoryAsync()
		{
			if (!File.Exists(_filePath))
				return Enumerable.Empty<string>();
		 using var reader = new StreamReader(_filePath);
		 List<string> historyEntries = new List<string>();
		 while (!reader.EndOfStream)
		 {
			 var line = await reader.ReadLineAsync();
			 if (line != null)
				 historyEntries.Add(line);
		 }
		 return historyEntries;
		}
	}
}