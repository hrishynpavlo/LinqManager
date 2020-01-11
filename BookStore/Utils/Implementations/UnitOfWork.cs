using BookStore.Db.Entities;
using BookStore.Utils.Interfaces;
using LinqManager.Client.Db;
using LinqManager.Client.Db.Entities;
using System.Threading.Tasks;

namespace BookStore.Utils.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db, IRepository<Country> countryRepository, IRepository<Author> authorRepository,
            IRepository<Book> bookRepository, IRepositoryAsync<PublishingHouse> phRepository)
        {
            _db = db;
            Countries = countryRepository;
            Authors = authorRepository;
            Books = bookRepository;
            PublishingHouse = phRepository;
        }

        public IRepository<Country> Countries { get; private set; }

        public IRepository<Author> Authors { get; private set; }

        public IRepository<Book> Books { get; private set; }

        public IRepositoryAsync<PublishingHouse> PublishingHouse { get; private set; }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}