using System;
using System.Collections.Generic;

namespace DomainModels.Models
{
    public class Film
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public ICollection<Genre> Genres { get; set; }
        public ICollection<Subscription> Subscriptions { get; set; }
        public decimal Rating { get; set; }
    }
}