using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStore.Models;
using BookStore.Models.DTOs;
using BookStore.Utils.Interfaces;
using LinqManager.Client.Db.Entities;
using LinqManager.Factories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Utils.Services
{
    public class CountryService: ICountryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CountryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateAsync(CountryDto country)
        {
            var dbCountry = _mapper.Map<Country>(country);
            await _unitOfWork.Countries.CreatAsync(dbCountry);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Countries.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<ObjectPagedModel<CountryDto>> GetAllAsync(string filterBy) 
        {
            var request = new DefaultRequestFactory(filterBy, null, 1, 10).CreateRequest();
            var countries = await _unitOfWork.Countries.GetAllAsync(request);
            return new ObjectPagedModel<CountryDto> (await countries.Items.ProjectTo<CountryDto>(_mapper.ConfigurationProvider).ToListAsync(), countries.Count);
        }

        public async Task<CountryDto> GetByIdAsync(int id)
        {
            var dbBook = await _unitOfWork.Countries.GetAsync(id);
            return _mapper.Map<CountryDto>(dbBook);
        }
    }
}
