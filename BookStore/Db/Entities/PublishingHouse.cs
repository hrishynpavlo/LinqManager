using System;
using System.Collections.Generic;

namespace BookStore.Db.Entities
{
    public class PublishingHouse
    {
        public PublishingHouse()
        {
            Books = new HashSet<BookPublishingHouse>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }

        public ICollection<BookPublishingHouse> Books { get; set; }
    }
}