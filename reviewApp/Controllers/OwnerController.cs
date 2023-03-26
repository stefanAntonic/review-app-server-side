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
    private readonly IPokemonRepository _pokemonRepository;
    private readonly IMapper _mapper;

    public OwnerController(IOwnerRepository ownerRepository, IPokemonRepository pokemonRepository, IMapper mapper)
    {
        _ownerRepository = ownerRepository;
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
    
}