using Company.App.Application.Shared;
using Company.App.Application.UseCases.DetectBatman.Models;
using Company.App.Application.UseCases.LookupHeroByName;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Application.UseCases.AddHero
{
    public record AddHeroCommand(HeroDto HeroDto) : IRequest<Result>;
}
