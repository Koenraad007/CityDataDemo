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
        public async Task<ActionResult> GetCountries()
        {
            var countries = await _mediator.Send(new GetAllCountriesQuery());
            return Ok(countries);
        }
    }
}