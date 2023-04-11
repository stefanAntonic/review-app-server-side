using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using reviewApp.Dto;
using reviewApp.Interfaces;
using reviewApp.Models;

namespace reviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]

public class OwnerController : Controller
{
    private readonly IOwnerRepository _ownerRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IPokemonRepository _pokemonRepository;
    private readonly IMapper _mapper;

    public OwnerController(
        IOwnerRepository ownerRepository,
        ICountryRepository countryRepository,
        IPokemonRepository pokemonRepository,
        IMapper mapper
        )
    {
        _ownerRepository = ownerRepository;
        _countryRepository = countryRepository;
        _pokemonRepository = pokemonRepository;
        _mapper = mapper;
    }
    
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]

    public IActionResult GetOwners()
    {
        var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwners());

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        return Ok(owners);

    }
    
    [HttpGet("{ownerId}")]
    [ProducesResponseType(200, Type = typeof(Owner))]
    [ProducesResponseType(400)]
    public IActionResult GetOwner(int ownerId)
    {
        if (!_ownerRepository.OwnerExisting(ownerId))
        {
            return NotFound();
        }
        var owner = _mapper.Map<OwnerDto>(_ownerRepository.GetOwner(ownerId));
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        return Ok(owner);
    }
    
    [HttpGet("{pokeId}/owner")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
    [ProducesResponseType(400)]
    public IActionResult GetOwnerByPokemonId(int pokeId)
    {
        if (!_pokemonRepository.PokemonExists(pokeId))
        {
            return NotFound();
        }
        var owner = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwnerOfAPokemon(pokeId));
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        return Ok(owner);
    }
    
    [HttpGet("{ownerId}/pokemon")]
    [ProducesResponseType(200, Type = typeof(Owner))]
    [ProducesResponseType(400)]
    public IActionResult GetPokemonByOwner(int ownerId)
    {
        if (!_ownerRepository.OwnerExisting(ownerId))
        {
            return NotFound();
        }
        var pokemon = _mapper.Map<List<PokemonDto>>(_ownerRepository.GetPokemonByOwner(ownerId));
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        return Ok(pokemon);
    }
    
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateOwner([FromQuery] int countryId, [FromBody] OwnerDto ownerCreate )
    {
        if ( ownerCreate == null)
        {
            return BadRequest();
        }
        
        // ReSharper disable once ReplaceWithSingleCallToFirstOrDefault
        var owner = _ownerRepository.GetOwners()
            .Where(item =>
                (item.LastName.Trim().ToLower() == ownerCreate.LastName.Trim().ToLower()
                 && 
                 item.FirstName == ownerCreate.FirstName))
            .FirstOrDefault();
        if ( owner != null) 
        {
            ModelState.AddModelError("", "Owner already exists");
            return StatusCode(442, ModelState);
        }

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var ownerMap = _mapper.Map<Owner>(ownerCreate);
        ownerMap.Country = _countryRepository.GetCountry(countryId);
        if (!_ownerRepository.CreateOwner(ownerMap))
        {
            ModelState.AddModelError("", "Something went wrong while saving");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }
    
    [HttpPut("{ownerId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult UpdateOwner(int ownerId, [FromQuery] int countryId, [FromBody] OwnerDto ownerUpdate )
    {
        if ( ownerUpdate == null)
        {
            return BadRequest(ModelState);
        }

        if (ownerId != ownerUpdate.Id)
        {
            return BadRequest(ModelState);

        }
        ;
        if ( !_ownerRepository.OwnerExisting(ownerId))
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        

        var ownerMap = _mapper.Map<Owner>(ownerUpdate);
        ownerMap.Country = _countryRepository.GetCountry(countryId);
        if (!_ownerRepository.UpdateOwner(ownerMap))
        {
            ModelState.AddModelError("", "Something went wrong while updating.");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully Updated");
    }
    
    [HttpDelete("{ownerId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult DeleteCategory(int ownerId )
    {
        var owner = _ownerRepository
            .GetOwners()
            .Where(owner1 => owner1.Id == ownerId)
            .FirstOrDefault();
        ;
        if (owner == null)
        {
            return NotFound(ModelState);
        }            
        if ( !_ownerRepository.OwnerExisting(ownerId))
        {
            return NotFound(ModelState);
        }

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        
        if (!_ownerRepository.DeleteOwner(owner))
        {
            ModelState.AddModelError("", "Something went wrong while deleting.");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully Deleted");
    }
    
}