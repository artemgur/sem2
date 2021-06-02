using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModels
{
    public class UserPermission
    {
        [Key]
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public User User { get; set; }
        
        [ForeignKey("Permission")]
        public string PermissionName { get; set; }
        public Permission Permission { get; set; }

        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }

        public int PermissionProviderId { get; set; }
        public SubscriptionPlan PermissionProvider { get; set; }
    }
}