using AP.CityDataDemo.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AP.CityDataDemo.Presentation.Controllers
{
    public class CityController : APIv1Controller
    {
        private readonly ICityService _cityService;
        private readonly IMediator _mediator;

        public CityController(ICityService cityService, IMediator mediator)
        {
            _cityService = cityService ?? throw new ArgumentNullException(nameof(cityService));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        public async Task<IActionResult> GetCities()
        {
            var cities = await _cityService.GetCitiesAsync();
            return Ok(cities);
        }

        [HttpGet("{cityId}")]
        public async Task<IActionResult> GetCity(int cityId, bool includePointsOfInterest = false)
        {
            var city = await _cityService.GetCityByIdAsync(cityId, includePointsOfInterest);
            if (city == null)
            {
                return NotFound();
            }
            return Ok(city);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCity([FromBody] Shared.DTO.CityDto city)
        {
            var createdCity = await _cityService.CreateCityAsync(city);
            return CreatedAtAction(nameof(GetCity), new { cityId = createdCity.Id }, createdCity);
        }

        [HttpPut("{cityId}")]
        public async Task<IActionResult> UpdateCity(int cityId, [FromBody] Shared.DTO.CityDto city)
        {
            var updated = await _cityService.UpdateCityAsync(cityId, city);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{cityId}")]
        public async Task<IActionResult> DeleteCity(int cityId)
        {
            var deleted = await _cityService.DeleteCityAsync(cityId);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}