using System.Threading.Tasks;
using BookStore.Models.DTOs;
using BookStore.Utils.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController, Route("api/publishingHouses")]
    public class PublishingHouseController : ControllerBase
    {
        private IPublishingHouseService _service;

        public PublishingHouseController(IPublishingHouseService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCountriesAsync([FromQuery]string filterBy = null)
        {
            return Ok(await _service.GetAllAsync(filterBy));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody]PublishingHouseDto publishingHouse)
        {
            await _service.CreateAsync(publishingHouse);
            return Ok();
        }
    }
}