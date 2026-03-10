using System;

namespace Company.App.Application.UseCases.DetectBatman.Models
{
    public record CheckRecordDto(int id, String fileName, String result, String processedAt);
}
