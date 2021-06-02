using System;
using System.ComponentModel.DataAnnotations;

namespace sem2.Infrastructure.Attributes
{
    public class DateGreaterThenCurrentAttribute : RangeAttribute
    {
        public DateGreaterThenCurrentAttribute()
            : base(typeof(DateTime), 
                DateTime.Now.ToShortDateString(), 
                DateTime.MaxValue.ToShortDateString())
        {
        }
    }
}