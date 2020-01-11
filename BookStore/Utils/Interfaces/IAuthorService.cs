using BookStore.Models;
using BookStore.Models.DTOs;
using System.Threading.Tasks;

namespace BookStore.Utils.Interfaces
{
    public interface IAuthorService
    {
        Task<ObjectPagedModel<AuthorDto>> GetAllAsync(string filterBy, string sortBy);
        Task CreateAsync(AuthorDto author);
        Task DeleteAsync(int id);
    }
}