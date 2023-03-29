using reviewApp.Data;
using reviewApp.Interfaces;
using reviewApp.Models;

namespace reviewApp.Repository;

public class ReviewerRepository : IReviewerRepository
{
    private readonly DataContext _context;

    public ReviewerRepository(DataContext context)
    {
        _context = context;
    }
    
    public ICollection<Reviewer> GetReviewers()
    {
        return _context
            .Reviwers
            .OrderBy(reviewer => reviewer.Id)
            .ToList();
    }

    public Reviewer GetReviewer(int reviewerId)
    {
        return _context
            .Reviwers
            .Where(reviewer => reviewer.Id == reviewerId)
            .FirstOrDefault();
    }

    public ICollection<Review> GetReviewsByReviewer(int reviewerId)
    {
        return _context
            .Reviews
            .Where(review => review.Reviewer.Id == reviewerId)
            .ToList();
    }

    public bool ReviewerExists(int reviewerId)
    {
        return _context
            .Reviwers
            .Any(reviewer => reviewer.Id == reviewerId);
    }
}