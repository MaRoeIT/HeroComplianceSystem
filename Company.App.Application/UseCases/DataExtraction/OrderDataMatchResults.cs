namespace Company.App.Application.UseCases.DataExtraction
{
    public record OrderDataMatchResults(bool Match, string Message, DateTime CheckedAt = default)
    {
        public string StatusColor => Match ? "Green" : "Red";
    }
}
