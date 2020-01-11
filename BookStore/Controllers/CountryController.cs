using System.Threading.Tasks;
using BookStore.Models.DTOs;
using BookStore.Utils.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController, Route("api/countries")]
    public class CountryController : ControllerBase
    {
        private ICountryService _service;

        public CountryController(ICountryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCountriesAsync([FromQuery]string filterBy = null)
        {
            return Ok(await _service.GetAllAsync(filterBy));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody]CountryDto country)
        {
            await _service.CreateAsync(country);
            return Ok();
        }
    }
}