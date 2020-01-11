using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStore.Db.Entities;
using BookStore.Models;
using BookStore.Models.DTOs;
using BookStore.Utils.Interfaces;
using LinqManager.Factories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookStore.Utils.Services
{
    public class PublishingHouseService : IPublishingHouseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PublishingHouseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateAsync(PublishingHouseDto publishingHouseDto)
        {
            var publishingHouse = _mapper.Map<PublishingHouse>(publishingHouseDto);
            await _unitOfWork.PublishingHouse.CreatAsync(publishingHouse);
            await _unitOfWork.SaveAsync();
        }

        public async Task<ObjectPagedModel<PublishingHouseDto>> GetAllAsync(string filterBy)
        {
            var request = new DefaultRequestFactory(filterBy, null, 1, 10).CreateRequest();
            var query = await _unitOfWork.PublishingHouse.GetAllAsync<PublishingHouseDto>(request);
            return new ObjectPagedModel<PublishingHouseDto>(await query.Items.ProjectTo<PublishingHouseDto>(_mapper.ConfigurationProvider).ToListAsync(), query.Count);
        }
    }
}