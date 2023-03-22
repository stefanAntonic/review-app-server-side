using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using reviewApp.Dto;
using reviewApp.Interfaces;
using reviewApp.Models;

namespace reviewApp.Controllers;


[Route("api/[controller]")]
[ApiController]
public class CountryController : Controller
{
    private readonly ICountryRepository _countryRepository;
    private readonly IMapper _mapper;

    public CountryController(ICountryRepository countryRepository, IMapper mapper)
    {
        _countryRepository = countryRepository;
        _mapper = mapper;
    }
    
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
    
    public IActionResult GetCategories()
    {
        var countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        return Ok(countries);
    }
    
    [HttpGet("{countryId}")]
    [ProducesResponseType(200, Type = typeof(Country))]
    [ProducesResponseType(400)]
    
    public IActionResult GetCountry(int countryId)
    {
        if (!_countryRepository.CountryExists(countryId))
        {
            return NotFound();
        }

        var country = _mapper.Map<CountryDto>(_countryRepository.GetCountry(countryId));

        if (!ModelState.IsValid && country.GetType().GetProperty("Id") == null)
        {
            return BadRequest();
        }

        return Ok(country);
    }
    
    [HttpGet("owner/{ownerId}")]
    [ProducesResponseType(200, Type = typeof(Country))]
    [ProducesResponseType(400)]
    
    public IActionResult GetCountryByOwnerId(int ownerId)
    {

        var country = _mapper.Map<CountryDto>(_countryRepository.GetCountryByOwnerId(ownerId));

        if (!ModelState.IsValid && country.GetType().GetProperty("Id") == null)
        {
            return BadRequest();
        }

        return Ok(country);
    }
}