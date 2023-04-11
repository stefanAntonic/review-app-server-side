using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using reviewApp.Dto;
using reviewApp.Interfaces;
using reviewApp.Models;

namespace reviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]

public class ReviewController : Controller
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IPokemonRepository _pokemonRepository;
    private readonly IMapper _mapper;
    private readonly IReviewerRepository _reviewerRepository;

    public ReviewController(
        IReviewRepository reviewRepository,
        IPokemonRepository pokemonRepository,
        IMapper mapper,
        IReviewerRepository reviewerRepository
    )
    {
        _reviewRepository = reviewRepository;
        _pokemonRepository = pokemonRepository;
        _mapper = mapper;
        _reviewerRepository = reviewerRepository;
    }
    
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
    public IActionResult GetReviews()
    {
        var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviews()) ;
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(reviews);
    }
    
    [HttpGet("{reviewId}")]
    [ProducesResponseType(200, Type = typeof(Review))]
    [ProducesResponseType(400)]
    public IActionResult GetPokemon(int reviewId)
    {
       
        if (!_reviewRepository.ReviewExists(reviewId)) 
        {
            return NotFound();
        }
        var review = _mapper.Map<ReviewDto>(_reviewRepository.GetReview(reviewId));
        if (!ModelState.IsValid && review.GetType().GetProperty("Id") == null)
        {
            return BadRequest(ModelState);
        }

        return Ok(review);
    }
    
    [HttpGet("{pokeId}/pokemonReviews")]
    [ProducesResponseType(200, Type = typeof(Review))]
    [ProducesResponseType(400)]
    public IActionResult GetPokemonReviews(int pokeId)
    {
       
        if (!_pokemonRepository.PokemonExists(pokeId)) 
        {
            return NotFound();
        }
        var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviewsOfAPokemon(pokeId));
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(reviews);
    }
    
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateCountry([FromQuery] int reviewerId, int pokemonId, [FromBody] ReviewDto reviewCreate )
    {
        if ( reviewCreate == null)
        {
            return BadRequest();
        }
        
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var reviewMap = _mapper.Map<Review>(reviewCreate);
        reviewMap.Pokemon = _pokemonRepository.GetPokemon(pokemonId);
        reviewMap.Reviewer = _reviewerRepository.GetReviewer(reviewerId);
        
        if (!_reviewRepository.CreateReview(reviewMap))
        {
            ModelState.AddModelError("", "Something went wrong while saving");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }
    
    [HttpPut("{reviewId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult UpdateReview(int reviewId, [FromBody] PokemonDto reviewUpdate )
    {
        if ( reviewUpdate == null)
        {
            return BadRequest(ModelState);
        }

        if (reviewId != reviewUpdate.Id)
        {
            return BadRequest(ModelState);

        }
        ;
        if ( !_reviewRepository.ReviewExists(reviewId))
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        

        var reviewMap = _mapper.Map<Review>(reviewUpdate);
        if (!_reviewRepository.UpdateReview(reviewMap))
        {
            ModelState.AddModelError("", "Something went wrong while updating.");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully Updated");
    }
    
    [HttpDelete("{reviewId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult DeleteReview(int reviewId )
    {
        var review = _reviewRepository
            .GetReview(reviewId);
        ;
        if (review == null)
        {
            return NotFound(ModelState);
        }            
        if ( !_reviewRepository.ReviewExists(reviewId))
        {
            return NotFound(ModelState);
        }

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        
        if (!_reviewRepository.DeleteReview(review))
        {
            ModelState.AddModelError("", "Something went wrong while deleting.");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully Deleted");
    }
    
    [HttpDelete("{reviewTitle}/deleteReviews")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult DeleteReviews(string reviewTitle )
    {
        var reviews = _reviewRepository
            .GetReviews()
            .Where(review => review.Title.ToLower() == reviewTitle.ToLower())
            .ToList();
        ;
        if (reviews == null)
        {
            return NotFound(ModelState);
        }

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        
        if (!_reviewRepository.DeleteReviews(reviews))
        {
            ModelState.AddModelError("", "Something went wrong while deleting.");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully Deleted");
    }
    
}