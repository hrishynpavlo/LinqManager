using System.Threading.Tasks;
using BookStore.Models.DTOs;
using BookStore.Utils.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController, Route("api/books")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _service;

        public BookController(IBookService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooksAsync([FromQuery]string filterBy = null)
        {
            return Ok(await _service.GetAllAsync(filterBy));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody]BookDto book)
        {
            await _service.CreateAsync(book);
            return Ok();
        }
    }
}