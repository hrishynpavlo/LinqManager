using System;
using LinqManager.Enums;

namespace LinqManager.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DtoMapAttribute: Attribute
    {
        public FilterMethods FilterMethod { get; private set; }
        public string DbPropertyName { get; private set; }

        public DtoMapAttribute(FilterMethods filterMethod = FilterMethods.OnlySortable, string dbPropertyName = null)
        {
            FilterMethod = filterMethod;
            DbPropertyName = dbPropertyName;
        }
    }
}