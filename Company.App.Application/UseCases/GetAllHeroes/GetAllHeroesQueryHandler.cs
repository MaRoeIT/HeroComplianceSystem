using Company.App.Application.Interfaces;
using Company.App.Application.Shared;
using Company.App.Application.UseCases.LookupHeroByName;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Application.UseCases.GetAllHeroes
{
    internal class GetAllHeroesQueryHandler :IRequestHandler<GetAllHeroesQuery,Result<List<HeroByNameResult>>>
    {
        private readonly IHeroRepository _heroRepository;
        public GetAllHeroesQueryHandler(IHeroRepository heroRepository)
        {
            _heroRepository = heroRepository;
        }

        public async Task<Result<List<HeroByNameResult>>> Handle(GetAllHeroesQuery query, CancellationToken cancellationToken)
        {
            var result = await _heroRepository.GetAllHeroesByNameAsync();
            return result;
        }
    }
}
