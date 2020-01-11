using BookStore.Models;
using BookStore.Models.DTOs;
using BookStore.Utils.Interfaces;
using LinqManager;
using LinqManager.Client.Db;
using LinqManager.Client.Db.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Utils.Implementations
{
    public class CountryRepository : IRepository<Country>
    {
        private readonly ApplicationDbContext _db;
        private readonly LinqManager.LinqManager _linqManager;

        public CountryRepository(ApplicationDbContext db, LinqManager.LinqManager linqManager)
        {
            _db = db;
            _linqManager = linqManager;
        }

        public async Task CreatAsync(Country item)
        {
            await _db.AddAsync(item);
        }

        public async Task DeleteAsync(int id)
        {
            var country = await GetAsync(id);
            _db.Countries.Remove(country);
        }

        public async Task<QueryPagedModel<Country>> GetAllAsync(LinqProccesRequest request)
        {
            int count;
            
            var items = _linqManager.Process<Country, CountryDto>(_db.Countries.AsQueryable(), request, out count);
            var result = new QueryPagedModel<Country>(items, count);
            return await Task.FromResult(result);
        }

        public async Task<Country> GetAsync(int id)
        {
            return await _db.Countries.FirstOrDefaultAsync(c => c.Id == id);
        }

        public Task UpdateAsync(Country item)
        {
            throw new NotImplementedException();
        }
    }
}
