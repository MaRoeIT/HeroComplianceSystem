using Company.App.Application.Interfaces;
using Company.App.Application.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Application.UseCases.LookupHeroByName
{
    internal class LookupHeroByNameQueryHandler: IRequestHandler<LookupHeroByNameQuery ,Result<HeroByNameResult>>
    {
        private readonly IHeroRepository _heroRepository;
        public LookupHeroByNameQueryHandler(IHeroRepository heroRepository) 
        {
            _heroRepository = heroRepository;
        }

        public async Task<Result<HeroByNameResult>> Handle(LookupHeroByNameQuery query, CancellationToken cancellationToken)
        {
            var result = await _heroRepository.GetHeroByNameAsync(query.Name);
            return result;
        }

    }
}
