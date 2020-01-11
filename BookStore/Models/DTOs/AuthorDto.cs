using LinqManager.Attributes;
using LinqManager.Enums;
using System;
using System.Collections.Generic;

namespace BookStore.Models.DTOs
{
    public class AuthorDto
    {
        public int Id { get; set; }
        [DtoMap(FilterMethods.StartWith)]
        public string FirstName { get; set; }

        [DtoMap(FilterMethods.StringContains)]
        public string LastName { get; set; }

        [DtoMap(FilterMethods.Range)]
        public DateTime DateOfBirth { get; set; }

        [DtoMap(FilterMethods.Equals, "Country.Id")]
        public DropDown<int> Country { get; set; }
        [DtoMap(FilterMethods.Any, "Books:Name")]
        public List<DropDown<int>> Books { get; set; } = new List<DropDown<int>>();
    }
}