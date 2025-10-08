using AP.CityDataDemo.Application.CQRS.Commands.Cities;
using AP.CityDataDemo.Application.CQRS.Queries.Cities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AP.CityDataDemo.Presentation.Controllers
{
    public class CityController : APIv1Controller
    {
        private readonly IMediator _mediator;

        public CityController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        public async Task<IActionResult> GetCities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetAllCitiesQuery { PageNumber = pageNumber, PageSize = pageSize };
            var cities = await _mediator.Send(query);
            return Ok(cities);
        }

        [HttpGet("{cityId}")]
        public async Task<IActionResult> GetCity(int cityId)
        {
            var city = await _mediator.Send(new GetCityByIdQuery(cityId));
            if (city == null)
            {
                return NotFound();
            }
            return Ok(city);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCity([FromBody] Shared.DTO.AddCityDto addCityDto)
        {
            var createdCity = await _mediator.Send(new CreateCityCommand(addCityDto));
            return CreatedAtAction(nameof(GetCity), new { cityId = createdCity.Id }, createdCity);
        }

        [HttpPut("{cityId}")]
        public async Task<IActionResult> UpdateCity(int cityId, [FromBody] Shared.DTO.CityDto city)
        {
            await _mediator.Send(new EditCityCommand(cityId, city.Name, (int)city.Population, city.CountryId));
            return NoContent();
        }

        [HttpDelete("{cityId}")]
        public async Task<IActionResult> DeleteCity(int cityId)
        {
            var deleted = await _mediator.Send(new DeleteCommand(cityId));
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}