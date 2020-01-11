using BookStore.Db.Entities;
using LinqManager.Client.Db.Entities;
using System.Threading.Tasks;

namespace BookStore.Utils.Interfaces
{
    public interface IUnitOfWork
    {
        Task SaveAsync();
        IRepository<Country> Countries { get; }
        IRepository<Author> Authors { get; }
        IRepository<Book> Books { get; }
        IRepositoryAsync<PublishingHouse> PublishingHouse { get; }
    }
}