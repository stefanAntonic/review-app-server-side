using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using reviewApp.Dto;
using reviewApp.Interfaces;
using reviewApp.Models;

namespace reviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]

public class ReviewerController : Controller
{
    private readonly IReviewerRepository _reviewerRepository;
    private readonly IMapper _mapper;

    public ReviewerController(IReviewerRepository reviewerRepository,  IMapper mapper)
    {
        _reviewerRepository = reviewerRepository;
        _mapper = mapper;
    }
    
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
    
    public IActionResult GetReviews()
    {
        var reviewers = _mapper.Map<List<ReviewerDto>>(_reviewerRepository.GetReviewers()) ;
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(reviewers);
    }
    
    [HttpGet("{reviewerId}")]
    [ProducesResponseType(200, Type = typeof(Reviewer))]
    [ProducesResponseType(400)]
    public IActionResult GetReviewer(int reviewerId)
    {
       
        if (!_reviewerRepository.ReviewerExists(reviewerId)) 
        {
            return NotFound();
        }
        var reviewer = _mapper.Map<ReviewerDto>(_reviewerRepository.GetReviewer(reviewerId));
        if (!ModelState.IsValid && reviewer.GetType().GetProperty("Id") == null)
        {
            return BadRequest(ModelState);
        }

        return Ok(reviewer);
    }
    
    [HttpGet("{reviewerId}/reviews")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
    [ProducesResponseType(400)]
    public IActionResult GetReviewsByReviewer(int reviewerId)
    {
       
        if (!_reviewerRepository.ReviewerExists(reviewerId)) 
        {
            return NotFound();
        }
        var reviews = _mapper.Map<List<ReviewDto>>(_reviewerRepository.GetReviewsByReviewer(reviewerId));
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(reviews);
    }
    
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateReviewer([FromBody] ReviewerDto reviewerCreate )
    {
        if ( reviewerCreate == null)
        {
            return BadRequest();
        }
        
        // ReSharper disable once ReplaceWithSingleCallToFirstOrDefault
        var owner = _reviewerRepository.GetReviewers()
            .Where(item =>
                (item.LastName.Trim().ToLower() == reviewerCreate.LastName.Trim().ToLower()
                 && 
                 item.FirstName == reviewerCreate.FirstName))
            .FirstOrDefault();
        if ( owner != null) 
        {
            ModelState.AddModelError("", "Rewiever already exists");
            return StatusCode(442, ModelState);
        }

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var reviewerMap = _mapper.Map<Reviewer>(reviewerCreate);
        if (!_reviewerRepository.CreateReviewer(reviewerMap))
        {
            ModelState.AddModelError("", "Something went wrong while saving");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }


}