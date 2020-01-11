using LinqManager.Client.Db.Entities;

namespace BookStore.Db.Entities
{
    public class BookPublishingHouse
    {
        public int BookId { get; set; }
        public int PublishingHouseId { get; set; }

        public Book Book { get; set; }
        public PublishingHouse PublishingHouse { get; set; }
    }
}