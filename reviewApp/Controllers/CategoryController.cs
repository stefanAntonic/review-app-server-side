using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
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

    [HttpGet("{categoryId}/pokemon")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
    [ProducesResponseType(400)]

    public IActionResult GetPokemon(int categoryId)
    {
        if (!_categoryRepository.CategoryExists(categoryId))
        {
            return NotFound();
        }

        var pokemons = _mapper.Map<List<PokemonDto>>(_categoryRepository.GetPokemonByCategory(categoryId));
        
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        return Ok(pokemons);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateCategory([FromBody] CategoryDto categoryCreate )
    {
        if ( categoryCreate == null)
        {
            return BadRequest();
        }

        var category = _categoryRepository.GetCategories()
            .Where(category1 => category1.Name.Trim().ToLower() == categoryCreate.Name.Trim().ToLower())
            .FirstOrDefault();
        if ( category != null) 
        {
            ModelState.AddModelError("", "Category already exists");
            return StatusCode(442, ModelState);
        }

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var categoryMap = _mapper.Map<Category>(categoryCreate);
        if (!_categoryRepository.CreateCategory(categoryMap))
        {
            ModelState.AddModelError("", "Something went wrong while saving");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }
    
    [HttpPut("{categoryId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto categoryUpdate )
    {
        if ( categoryUpdate == null)
        {
            return BadRequest(ModelState);
        }

        if (categoryId != categoryUpdate.Id)
        {
            return BadRequest(ModelState);

        }
        ;
        if ( !_categoryRepository.CategoryExists(categoryId))
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        

        var categoryMap = _mapper.Map<Category>(categoryUpdate);
        if (!_categoryRepository.UpdateCategory(categoryMap))
        {
            ModelState.AddModelError("", "Something went wrong while updating.");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully Updated");
    }
    
    [HttpDelete("{categoryId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult DeleteCategory(int categoryId )
    {
        var category = _categoryRepository
            .GetCategories()
            .Where(catgory1 => catgory1.Id == categoryId)
            .FirstOrDefault();
            ;
            if (category == null)
            {
                return NotFound(ModelState);
            }            
        if ( !_categoryRepository.CategoryExists(categoryId))
        {
            return NotFound(ModelState);
        }

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        
        if (!_categoryRepository.DeleteCategory(category))
        {
            ModelState.AddModelError("", "Something went wrong while deleting.");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully Deleted");
    }
    
}