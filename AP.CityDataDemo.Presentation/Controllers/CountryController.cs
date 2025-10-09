using MediatR;
using Microsoft.AspNetCore.Mvc;
using AP.CityDataDemo.Application.CQRS.Queries.Countries;

namespace AP.CityDataDemo.Presentation.Controllers
{
    public class CountryController : APIv1Controller
    {
        private readonly IMediator _mediator;

        public CountryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GetCountries([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetAllCountriesQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var countries = await _mediator.Send(query);
            return Ok(countries);
        }
    }
}