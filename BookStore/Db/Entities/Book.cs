using BookStore.Db.Entities;
using System;
using System.Collections.Generic;

namespace LinqManager.Client.Db.Entities
{
    public class Book
    {
        public Book()
        {
            PublishingHouses = new HashSet<BookPublishingHouse>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime PublishedDate { get; set; }
        public int AuthorId { get; set; }

        public Author Author { get; set; }
        public ICollection<BookPublishingHouse> PublishingHouses { get; set; }
    }
}