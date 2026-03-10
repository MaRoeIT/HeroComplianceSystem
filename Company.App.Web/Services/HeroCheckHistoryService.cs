using Company.App.Application.UseCases.DetectBatman;

namespace Company.App.Web.Services;

public class HeroCheckHistoryService
{
    private readonly List<HistoryEntry> _history = new();

    public void AddEntry(DetectionResult result, string fileName)
    {
        _history.Add(new HistoryEntry
        {
            Id = Guid.NewGuid(),
            FileName = fileName,
            Result = result,
            ProcessedAt = result.CheckedAt != default ? result.CheckedAt : DateTime.UtcNow
        });
    }

    public IReadOnlyList<HistoryEntry> GetHistory() => _history.AsReadOnly();

    public void ClearHistory() => _history.Clear();
}

public class HistoryEntry
{
    public Guid Id { get; init; }
    public string FileName { get; init; } = string.Empty;
    public DetectionResult Result { get; init; } = null!;
    public DateTime ProcessedAt { get; init; }
}
