using BookStore.Models;
using BookStore.Models.DTOs;
using System.Threading.Tasks;

namespace BookStore.Utils.Interfaces
{
    public interface IPublishingHouseService
    {
        Task CreateAsync(PublishingHouseDto publishingHouseDto);
        Task<ObjectPagedModel<PublishingHouseDto>> GetAllAsync(string filterBy);
    }
}