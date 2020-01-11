using BookStore.Models;
using LinqManager;
using System.Threading.Tasks;

namespace BookStore.Utils.Interfaces
{
    public interface IRepository<T> where T: class
    {
        Task<QueryPagedModel<T>> GetAllAsync(LinqProccesRequest request);
        Task<T> GetAsync(int id);
        Task CreatAsync(T item);
        Task UpdateAsync(T item);
        Task DeleteAsync(int id);
    }
}