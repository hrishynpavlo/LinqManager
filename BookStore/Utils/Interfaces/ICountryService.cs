using BookStore.Models;
using BookStore.Models.DTOs;
using System.Threading.Tasks;

namespace BookStore.Utils.Interfaces
{
    public interface ICountryService
    {
        Task<ObjectPagedModel<CountryDto>> GetAllAsync(string filterBy);
        Task CreateAsync(CountryDto country);
        Task DeleteAsync(int id);
        Task<CountryDto> GetByIdAsync(int id);
    }
}