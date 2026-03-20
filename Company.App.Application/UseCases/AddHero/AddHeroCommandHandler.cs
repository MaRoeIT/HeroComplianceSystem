using Company.App.Application.Interfaces;
using Company.App.Application.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Application.UseCases.AddHero
{
    public class AddHeroCommandHandler : IRequestHandler<AddHeroCommand, Result>
    {
        private readonly IHeroRepository _heroRepository;

        public AddHeroCommandHandler(IHeroRepository heroRepository)
        {
            _heroRepository = heroRepository;
        }

        public async Task<Result> Handle(AddHeroCommand command, CancellationToken cancellationToken)
        {
            var result = await _heroRepository.AddHeroAsync(command.HeroDto);
            return result;
        }
    }
}
