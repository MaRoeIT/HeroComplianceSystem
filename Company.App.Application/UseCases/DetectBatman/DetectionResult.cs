namespace Company.App.Application.UseCases.DetectBatman
{
    public record DetectionResult(bool Found, string Message, DateTime CheckedAt = default)
    {
        public string StatusColor => Found ? "Green" : "Red";
    }
}
