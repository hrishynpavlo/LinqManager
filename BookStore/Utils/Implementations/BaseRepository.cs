using BookStore.Models;
using BookStore.Utils.Interfaces;
using LinqManager;
using LinqManager.Client.Db;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Utils.Implementations
{
    public class BaseRepository<TDb> : IRepositoryAsync<TDb> where TDb : class
    {
        private readonly ApplicationDbContext _db;
        private readonly LinqManager.LinqManager _linqManager;

        public BaseRepository(ApplicationDbContext db, LinqManager.LinqManager linqManager)
        {
            _db = db;
            _linqManager = linqManager;
        }

        public virtual async Task CreatAsync(TDb item)
        {
            await _db.Set<TDb>().AddAsync(item);
        }

        public virtual async Task DeleteAsync(int id)
        {
            var item = await GetAsync(id);
            _db.Set<TDb>().Remove(item);
        }

        public virtual async Task<QueryPagedModel<TDb>> GetAllAsync<TDto>(LinqProccesRequest request) where TDto: class
        {
            int count;
            var query = _linqManager.Process<TDb, TDto>(_db.Set<TDb>().AsQueryable(), request, out count);
            return await Task.FromResult(new QueryPagedModel<TDb>(query, count));
        }

        public virtual async Task<TDb> GetAsync(int id)
        {
            return await _db.Set<TDb>().FindAsync(id);
        }

        public virtual async Task UpdateAsync(TDb item)
        {
            _db.Set<TDb>().Update(item);
            await Task.CompletedTask;
        }
    }
}