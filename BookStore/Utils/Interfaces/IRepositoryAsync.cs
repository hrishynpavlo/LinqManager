using BookStore.Models;
using LinqManager;
using System.Threading.Tasks;

namespace BookStore.Utils.Interfaces
{
    public interface IRepositoryAsync<TDb> where TDb : class
    {
        Task<QueryPagedModel<TDb>> GetAllAsync<TDto>(LinqProccesRequest request) where TDto: class;
        Task<TDb> GetAsync(int id);
        Task CreatAsync(TDb item);
        Task UpdateAsync(TDb item);
        Task DeleteAsync(int id);
    }
}