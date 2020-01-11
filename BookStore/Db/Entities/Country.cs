using System.Collections.Generic;

namespace LinqManager.Client.Db.Entities
{
    public class Country
    {
        public Country()
        {
            Authors = new HashSet<Author>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Author> Authors { get; set; }
    }
}
