using System;
using System.ComponentModel.DataAnnotations;
using sem2.Infrastructure.Attributes;

namespace sem2
{
    public class ExpirationDate
    {
        [Required]
        [Range(1, 12)]
        public int ExpirationMonth { get; set; }

        [Required]
        [Range(2000, 9999)]
        public int ExpirationYear { get; set; }
    }

    public class CreditCardInfo
    {
        [Required]
        [DataType(DataType.CreditCard)]
        [CreditCard]
        public string Number { get; set; }

        [Required]
        [Range(100, 999)]
        public int CCV { get; set; }

        [Required]
        public ExpirationDate ExpirationDate { get; set; }
    }
}