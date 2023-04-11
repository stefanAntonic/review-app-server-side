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
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateCountry([FromBody] CountryDto countryCreate )
    {
        if ( countryCreate == null)
        {
            return BadRequest();
        }

        var country = _countryRepository.GetCountries()
            .Where(item => item.Name.Trim().ToLower() == countryCreate.Name.Trim().ToLower())
            .FirstOrDefault();
        if ( country != null) 
        {
            ModelState.AddModelError("", "Category already exists");
            return StatusCode(442, ModelState);
        }

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var countryMap = _mapper.Map<Country>(countryCreate);
        if (!_countryRepository.CreateCountry(countryMap))
        {
            ModelState.AddModelError("", "Something went wrong while saving");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }
    
    [HttpPut("{countryId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult UpdateCountry(int countryId, [FromBody] CountryDto countryUpdate )
    {
        if ( countryUpdate == null)
        {
            return BadRequest(ModelState);
        }

        if (countryId != countryUpdate.Id)
        {
            return BadRequest(ModelState);

        }
        ;
        if ( !_countryRepository.CountryExists(countryId))
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        

        var countryMap = _mapper.Map<Country>(countryUpdate);
        if (!_countryRepository.UpdateCountry(countryMap))
        {
            ModelState.AddModelError("", "Something went wrong while updating.");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully Updated");
    }
    
    [HttpDelete("{countryId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult DeleteCategory(int countryId )
    {
        var country = _countryRepository
            .GetCountries()
            .Where(country1 => country1.Id == countryId)
            .FirstOrDefault();
        ;
        if (country == null)
        {
            return NotFound(ModelState);
        }            
        if ( !_countryRepository.CountryExists(countryId))
        {
            return NotFound(ModelState);
        }

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        
        if (!_countryRepository.DeleteCountry(country))
        {
            ModelState.AddModelError("", "Something went wrong while deleting.");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully Deleted");
    }
}