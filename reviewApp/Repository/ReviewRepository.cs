using reviewApp.Data;
using reviewApp.Interfaces;
using reviewApp.Models;

namespace reviewApp.Repository;

public class ReviewRepository : IReviewRepository
{
    private readonly DataContext _context;

    public ReviewRepository(DataContext context)
    {
        _context = context;
    }
    public ICollection<Review> GetReviews()
    {
        var reviews = _context
            .Reviews
            .OrderByDescending(review => review.Rating)
            .ToList();
        return reviews;
    }

    public Review GetReview(int reviewId)
    {
        Review review = _context
            .Reviews
            .Where(review1 => review1.Id == reviewId)
            .FirstOrDefault();
        return review;
    }

    public ICollection<Review> GetReviewsOfAPokemon(int pokeId)
    {
        var pokemonReviews = _context
            .Reviews
            .Where(review => review.Pokemon.Id == pokeId)
            .ToList();
        return pokemonReviews;
    }

    public bool ReviewExists(int reviewId)
    {
        return _context.Reviews.Any(review => review.Id == reviewId);
    }

    public bool CreateReview(Review review)
    {
        _context.Add(review);
        return Save();
    }

    public bool Save()
    {
        return _context.SaveChanges() > 0;
        
    }
}