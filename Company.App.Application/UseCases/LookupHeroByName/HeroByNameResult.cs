using System;
using System.Collections.Generic;
using System.Text;

namespace Company.App.Application.UseCases.LookupHeroByName
{
    public record HeroByNameResult(bool Found, string Name, string SecretIdentity, DateTime LastUpdatedAt = default);
}
