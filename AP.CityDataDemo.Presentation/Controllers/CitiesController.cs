using Microsoft.AspNetCore.Mvc;
using MediatR;
using AP.CityDataDemo.Application.DTOs;
using AP.CityDataDemo.Application.CQRS.Queries.Cities;
using AP.CityDataDemo.Application.CQRS.Queries.Countries;
using AP.CityDataDemo.Application.CQRS.Commands.Cities;
using FluentValidation;
using AP.MyGameStore.Application.CQRS.Queries.Cities;

namespace AP.CityDataDemo.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CitiesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CitiesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityDto>>> GetAllCities()
    {
        var cities = await _mediator.Send(new GetAllCitiesQuery());
        return Ok(cities);
    }

    [HttpGet("sorted-by-population")]
    public async Task<ActionResult<IEnumerable<CityDto>>> GetCitiesSortedByPopulation([FromQuery] bool descending = false)
    {
        var cities = await _mediator.Send(new GetCitiesSortedByPopulationQuery(descending));
        return Ok(cities);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CityDto>> GetCity(int id)
    {
        var city = await _mediator.Send(new GetCityByIdQuery(id));
        if (city == null)
        {
            return NotFound();
        }
        return Ok(city);
    }

    [HttpPost]
    public async Task<ActionResult<CityDto>> CreateCity(AddCityDto addCityDto)
    {
        try
        {
            var createdCity = await _mediator.Send(new CreateCityCommand(addCityDto));
            return CreatedAtAction(nameof(GetCity), new { id = createdCity.Id }, createdCity);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Errors);
        }
    }


    [HttpGet("countries")]
    public async Task<ActionResult<IEnumerable<CountryDto>>> GetAllCountries()
    {
        var countries = await _mediator.Send(new GetAllCountriesQuery());
        return Ok(countries);
    }
}
