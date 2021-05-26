using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DomainModels
{
    public class SubscriptionPlan
    {
        [Key]
        public int Id { get; set; }
        
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public ICollection<Permission> ProvidedPermissions { get; set; }
    }
}