using System.Threading.Tasks;
using BookStore.Models.DTOs;
using BookStore.Utils.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController, Route("api/authors")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _service;
        private readonly IEmailService _emailService;

        public AuthorController(IAuthorService service, IEmailService emailService)
        {
            _service = service;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAuthorsAsync([FromQuery]string filterBy = null, [FromQuery] string sortBy = null)
        {
            return Ok(await _service.GetAllAsync(filterBy, sortBy));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody]AuthorDto author)
        {
            await _service.CreateAsync(author);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute]int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}