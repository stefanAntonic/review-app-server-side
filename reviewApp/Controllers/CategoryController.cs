using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using reviewApp.Dto;
using reviewApp.Interfaces;
using reviewApp.Models;

namespace reviewApp.Controllers;


[Route("api/[controller]")]
[ApiController]
public class CategoryController : Controller
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]

    public IActionResult GetCategories()
    {
        var categories = _mapper.Map<List<CategoryDto>>(_categoryRepository.GetCategories());

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        return Ok(categories);
    }

    [HttpGet("{categoryId}")]
    [ProducesResponseType(200, Type = typeof(Category))]
    [ProducesResponseType(400)]

    public IActionResult GetCategory(int categoryId)
    {
        if (!_categoryRepository.CategoryExists(categoryId))
        {
            return NotFound();
        }

        var category = _mapper.Map<CategoryDto>(_categoryRepository.GetCategory(categoryId));

        if (!ModelState.IsValid && category.GetType().GetProperty("Id") == null)
        {
            return BadRequest();
        }

        return Ok(category);
    }

    [HttpGet("pokemon/{categoryId}")]
    [ProducesResponseType(200, Type = typeof(Category))]
    [ProducesResponseType(400)]

    public IActionResult GetPokemon(int categoryId)
    {
        if (!_categoryRepository.CategoryExists(categoryId))
        {
            return NotFound();
        }

        var pokemon = _mapper.Map<List<PokemonDto>>(_categoryRepository.GetPokemonByCategory(categoryId));
        
        if (!ModelState.IsValid && pokemon.GetType().GetProperty("Id") == null)
        {
            return BadRequest();
        }

        return Ok(pokemon);
    }
    
}