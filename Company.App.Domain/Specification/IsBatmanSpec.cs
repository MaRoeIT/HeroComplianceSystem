namespace Company.App.Domain.Specification
{
    public class IsBatmanSpec
    {
        public bool IsBatman(string heroName, string secretIdentity)
        {
            return heroName.Equals("Batman", StringComparison.OrdinalIgnoreCase)
               && secretIdentity.Equals("Bruce Wayne", StringComparison.OrdinalIgnoreCase);
        }
    }
}
