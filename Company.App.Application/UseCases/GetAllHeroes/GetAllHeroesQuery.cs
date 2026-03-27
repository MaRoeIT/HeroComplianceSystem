using Company.App.Application.Shared;
using Company.App.Application.UseCases.LookupHeroByName;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Application.UseCases.GetAllHeroes
{
    public record GetAllHeroesQuery() : IRequest<Result<List<HeroByNameResult>>>;
}
