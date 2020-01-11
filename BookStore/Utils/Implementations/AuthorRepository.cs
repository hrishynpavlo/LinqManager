using BookStore.Utils.Interfaces;
using LinqManager.Client.Db.Entities;
using LinqManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqManager.Client.Db;
using BookStore.Models;
using BookStore.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using LinqManager.EFCoreExtensions;

namespace BookStore.Utils.Implementations
{
    public class AuthorRepository : IRepository<Author>
    {
        private readonly ApplicationDbContext _db;
        private readonly LinqManager.LinqManager _linqManager;

        public AuthorRepository(ApplicationDbContext db, LinqManager.LinqManager linqManager)
        {
            _db = db;
            _linqManager = linqManager;
        }

        public async Task CreatAsync(Author item)
        {
            await _db.AddAsync(item);
        }

        public async Task DeleteAsync(int id)
        {
            var author = await GetAsync(id);
            _db.Authors.Remove(author);
        }

        public async Task<QueryPagedModel<Author>> GetAllAsync(LinqProccesRequest request)
        {
            var items = await _linqManager.ProcessAsync<Author, AuthorDto>(_db.Authors, request);
            var result = new QueryPagedModel<Author>(items.Query, items.Count);

            return result;
        }

        public async Task<Author> GetAsync(int id)
        {
            return await _db.Authors.FirstOrDefaultAsync(a => a.Id == id);
        }

        public Task UpdateAsync(Author item)
        {
            throw new NotImplementedException();
        }
    }
}
