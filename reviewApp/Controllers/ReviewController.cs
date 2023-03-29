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

    public ReviewController(IReviewRepository reviewRepository, IPokemonRepository pokemonRepository, IMapper mapper)
    {
        _reviewRepository = reviewRepository;
        _pokemonRepository = pokemonRepository;
        _mapper = mapper;
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
}