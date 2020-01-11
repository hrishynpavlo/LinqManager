using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStore.Models;
using BookStore.Models.DTOs;
using BookStore.Utils.Interfaces;
using LinqManager.Client.Db.Entities;
using LinqManager.Factories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookStore.Utils.Services
{
    public class AuthorService : IAuthorService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public AuthorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateAsync(AuthorDto author)
        {
            var dbAuthor = _mapper.Map<Author>(author);
            await _unitOfWork.Authors.CreatAsync(dbAuthor);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Authors.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<ObjectPagedModel<AuthorDto>> GetAllAsync(string filterBy, string sortBy)
        {
            var request = new DefaultRequestFactory(filterBy, sortBy, 1, 10).CreateRequest();
            var query = await _unitOfWork.Authors.GetAllAsync(request);
            return new ObjectPagedModel<AuthorDto>(await query.Items.ProjectTo<AuthorDto>(_mapper.ConfigurationProvider).ToListAsync(), query.Count);
        }
    }
}
