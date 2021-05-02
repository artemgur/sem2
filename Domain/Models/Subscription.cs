using System.Collections.Generic;

namespace DomainModels.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public ICollection<Film> Films { get; set; }

        private Subscription()
        {
        }
    }
}