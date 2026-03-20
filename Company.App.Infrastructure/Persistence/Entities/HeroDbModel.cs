using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Company.App.Infrastructure.Persistence.Entities
{
    public class HeroDbModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public string SecretIdentity { get; set; } = string.Empty;
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        public HeroDbModel(string name, string secretIdentity) 
        {
            this.Name = name;
            this.SecretIdentity = secretIdentity;
        }
    }
}
