using System;
using System.Collections.Generic;

namespace BookStore.Models.DTOs
{
    public class PublishingHouseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<DropDown<int>> Books { get; set; }
    }
}