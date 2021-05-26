using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DomainModels
{
    public class Permission
    {
        [Key]
        public string Name { get; set; }
        public string Description { get; set; }
        
        public ICollection<SubscriptionPlan> ProvidedBy { get; set; }
    }
}