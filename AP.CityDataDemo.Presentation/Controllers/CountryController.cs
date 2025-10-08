using AP.CityDataDemo.Application.DTOs;
using AP.CityDataDemo.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AP.CityDataDemo.Presentation.Controllers
{
    public class CountryController : APIv1Controller
    {
        private readonly ICountryService _countryService;
        private readonly IMediator _mediator;

        public CountryController(ICountryService countryService, IMediator mediator)
        {
            _countryService = countryService;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GetCountries()
        {
            var countries = await _countryService.GetCountriesAsync();
            return Ok(countries);
        }

        [HttpGet("{countryId}")]
        public async Task<ActionResult> GetById(int countryId, bool includeCities = false)
        {
            var country = await _countryService.GetCountryByIdAsync(countryId, includeCities);
            if (country == null)
            {
                return NotFound();
            }
            return Ok(country);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CountryDto country)
        {
            var createdCountry = await _countryService.CreateCountryAsync(country);
            return CreatedAtAction(nameof(GetById), new { countryId = createdCountry.Id }, createdCountry);
        }

        [HttpPut("{countryId}")]
        public async Task<ActionResult> Update(int countryId, [FromBody] CountryDto country)
        {
            var updated = await _countryService.UpdateCountryAsync(countryId, country);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{countryId}")]
        public async Task<ActionResult> Delete(int countryId)
        {
            var deleted = await _countryService.DeleteCountryAsync(countryId);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}