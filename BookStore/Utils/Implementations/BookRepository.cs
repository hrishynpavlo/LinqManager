using BookStore.Models;
using BookStore.Utils.Interfaces;
using LinqManager.Client.Db.Entities;
using LinqManager;
using System;
using System.Threading.Tasks;
using LinqManager.Client.Db;
using BookStore.Models.DTOs;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Utils.Implementations
{
    public class BookRepository : IRepository<Book>
    {
        private readonly ApplicationDbContext _db;
        private readonly LinqManager.LinqManager _linqManager;

        public BookRepository(ApplicationDbContext db, LinqManager.LinqManager linqManager)
        {
            _db = db;
            _linqManager = linqManager;
        }

        public async Task CreatAsync(Book item)
        {
            await _db.Books.AddAsync(item);
        }

        public async Task DeleteAsync(int id)
        {
            var book = await GetAsync(id);
            _db.Books.Remove(book);
        }

        public async Task<QueryPagedModel<Book>> GetAllAsync(LinqProccesRequest request)
        {
            int count;

            var query = _linqManager.Process<Book, BookDto>(_db.Books.AsQueryable(), request, out count);
            var result = new QueryPagedModel<Book>(query, count);

            return await Task.FromResult(result);
        }

        public async Task<Book> GetAsync(int id)
        {
            return await _db.Books.FirstOrDefaultAsync(b => b.Id == id);
        }

        public Task UpdateAsync(Book item)
        {
            throw new NotImplementedException();
        }
    }
}