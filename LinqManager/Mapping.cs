using LinqManager.Enums;

namespace LinqManager
{
    class Mapping
    {
        public string DtoPropertyName { get; set; }
        public string DbPropertyName { get; set; }
        public string DbCollectionPropertyName { get; set; }
        public FilterMethods FilterMethod { get; set; }
    }
}