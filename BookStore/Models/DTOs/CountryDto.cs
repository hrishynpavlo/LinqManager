using LinqManager.Attributes;
using LinqManager.Enums;

namespace BookStore.Models.DTOs
{
    public class CountryDto
    {
        public int Id { get; set; }
        [DtoMap(FilterMethods.StartWith)]
        public string Name { get; set; }
    }
}