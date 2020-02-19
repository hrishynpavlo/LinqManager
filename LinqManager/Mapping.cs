using LinqManager.Enums;

namespace LinqManager.Core
{
    class Mapping
    {
        public string DtoPropertyName { get; set; }
        public string DbPropertyName { get; set; }
        public string DbCollectionPropertyName { get; set; }
        public FilterMethods FilterMethod { get; set; }
        public LogicalOperator LogicalOperator { get; set; }
    }
}