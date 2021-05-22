using System;

namespace sem2.Infrastructure.Filters
{
    public class ForPropertyAttribute : Attribute
    {
        public int PropertyId;

        public ForPropertyAttribute(int propertyId)
        {
            PropertyId = propertyId;
        }
    }
}