using System;

namespace sem2.ViewModels.SubscriptionModels
{
    public class SubscriptionViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        
        public decimal Price { get; set; }
        
        public int Duration { get; set; }
        public bool IsUserSubscribed { get; set; }
        public DateTime EndPeriodDate { get; set; }
        public int Id { get; set; }
    }
}