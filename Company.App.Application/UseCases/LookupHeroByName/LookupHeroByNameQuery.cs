using Company.App.Application.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Application.UseCases.LookupHeroByName
{
    public record LookupHeroByNameQuery(string Name): IRequest<Result<HeroByNameResult>>;
}
