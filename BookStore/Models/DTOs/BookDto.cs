using System;
using System.Collections.Generic;

namespace BookStore.Models.DTOs
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime PublishedDate { get; set; }
        public DropDown<int> Author { get; set; }
        public List<DropDown<int>> PublishingHouses { get; set; }
    }
}