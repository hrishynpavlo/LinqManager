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
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateAsync(BookDto book)
        {
            var dbBook = _mapper.Map<Book>(book);
            await _unitOfWork.Books.CreatAsync(dbBook);
            await _unitOfWork.SaveAsync();
        }

        public async Task<ObjectPagedModel<BookDto>> GetAllAsync(string filterBy)
        {
            var request = new DefaultRequestFactory(filterBy, null, 1, 10).CreateRequest();
            var query = await _unitOfWork.Books.GetAllAsync(request);
            return new ObjectPagedModel<BookDto>(await query.Items.ProjectTo<BookDto>(_mapper.ConfigurationProvider).ToListAsync(), query.Count);
        }
    }
}
