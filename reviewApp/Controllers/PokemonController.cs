using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using reviewApp.Dto;
using reviewApp.Interfaces;
using reviewApp.Models;

namespace reviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PokemonController : Controller
{
    private readonly IPokemonRepository _pokemonRepository;
    private readonly IMapper _mapper;

    public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper)
    {
        _pokemonRepository = pokemonRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
    public IActionResult GetPokemons()
    {
        var pokemons = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons()) ;
         if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(pokemons);
    }

    [HttpGet("{pokeId}")]
    [ProducesResponseType(200, Type = typeof(Pokemon))]
    [ProducesResponseType(400)]
    public IActionResult GetPokemon(int pokeId)
    {
       
        if (!_pokemonRepository.PokemonExists(pokeId)) 
        {
            return NotFound();
        }
        var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(pokeId));
        if (!ModelState.IsValid && pokemon.GetType().GetProperty("Id") == null)
        {
            return BadRequest(ModelState);
        }

        return Ok(pokemon);
    }

    [HttpGet("{pokeId}/rating")]
    [ProducesResponseType(200, Type = typeof(decimal))]
    [ProducesResponseType(400)]

    public IActionResult GetPokemonRating(int pokeId)
    {
        if (!_pokemonRepository.PokemonExists(pokeId))
        {
            return NotFound();
        }

        var rating = _pokemonRepository.GetPokemonRating(pokeId);

        if (!ModelState.IsValid && rating.GetType().GetProperty("Id") == null)
        {
            return BadRequest();
        }

        return Ok(Math.Round(rating, 2));
    }
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreatePokemon([FromQuery] int ownerId, int categoryId, [FromBody] PokemonDto pokemonCreate )
    {
        Console.WriteLine(pokemonCreate);
        if ( pokemonCreate == null)
        {
            return BadRequest();
        }

        // ReSharper disable once ReplaceWithSingleCallToFirstOrDefault
        var pokemon = _pokemonRepository.GetPokemons()
            .Where(item => item.Name.Trim().ToLower() == pokemonCreate.Name.Trim().ToLower())
            .FirstOrDefault();
        if ( pokemon != null) 
        {
            ModelState.AddModelError("", "Category already exists");
            return StatusCode(442, ModelState);
        }

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);
        if (!_pokemonRepository.CreatePokemon(ownerId, categoryId, pokemonMap))
        {
            ModelState.AddModelError("", "Something went wrong while saving");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }
}