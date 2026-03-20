using Company.App.Application.Shared;
using Company.App.Application.UseCases.DetectBatman;
using Company.App.Application.UseCases.DetectBatman.Models;
using Company.App.Application.UseCases.LookupHeroByName;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Application.Interfaces
{
    public interface IHeroRepository
    {
        Task<Result<HeroByNameResult>>GetHeroByNameAsync(string name);
        Task<Result> AddHeroAsync(HeroDto entity);
    }
}
