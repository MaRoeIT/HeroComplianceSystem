using Company.App.Application.Interfaces;
using Company.App.Application.Shared;
using Company.App.Application.UseCases.DetectBatman;
using Company.App.Application.UseCases.DetectBatman.Models;
using Company.App.Application.UseCases.LookupHeroByName;
using Company.App.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Company.App.Infrastructure.Persistence.Repositories
{
    public class HeroRepository: IHeroRepository
    {
        private readonly AppDbContext _context;
        public HeroRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<HeroByNameResult>>GetHeroByNameAsync(string name)
        {
            HeroDbModel? dbHero = await _context.Heroes.FirstOrDefaultAsync(hero => hero.Name == name);
            if (dbHero == null) return new Result<HeroByNameResult>(new HeroByNameResult(false, null, null, DateTime.MinValue), true);

            return new Result<HeroByNameResult>(new HeroByNameResult(true,dbHero.Name,dbHero.SecretIdentity,dbHero.LastUpdated),true);
        }

        public async Task<Result<List<HeroByNameResult>>>GetAllHeroesByNameAsync()
        {
            List<HeroDbModel> dbHeroes = await _context.Heroes.ToListAsync<HeroDbModel>();

            List<HeroByNameResult> result = new();

            foreach(var dbHero in dbHeroes)
            {
                result.Add(new HeroByNameResult(true, dbHero.Name, dbHero.SecretIdentity, dbHero.LastUpdated));
            }

            return new Result<List<HeroByNameResult>>(result, true);
        }

        public async Task<Result> AddHeroAsync(HeroDto entity)
        {
            HeroDbModel dbHero = new(entity.Name, entity.Identity);
            _context.Heroes.Add(dbHero);
            await _context.SaveChangesAsync();

            return new Result(true);
        }
    }
}
