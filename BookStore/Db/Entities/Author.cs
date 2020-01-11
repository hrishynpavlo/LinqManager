using System;
using System.Collections.Generic;
using System.Text;

namespace LinqManager.Client.Db.Entities
{
    public class Author
    {
        public Author()
        {
            Books = new HashSet<Book>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int CountryId { get; set; }

        public Country Country { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
