using BookStore.Models;
using BookStore.Models.DTOs;
using System.Threading.Tasks;

namespace BookStore.Utils.Interfaces
{
    public interface IBookService
    {
        Task CreateAsync(BookDto book);
        Task<ObjectPagedModel<BookDto>> GetAllAsync(string filterBy);
    }
}