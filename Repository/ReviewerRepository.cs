using PokemonReviewApi.Data;
using PokemonReviewApi.Interfaces;
using PokemonReviewApi.Models;

namespace PokemonReviewApi.Repository
{
  public class ReviewerRepository : IReviewerRepository
  {
    private readonly DataContext _context;

    public ReviewerRepository(DataContext context)
    {
      _context = context;
    }

    public ICollection<Review> GetReviewsByReviewer(int reviewerId)
    {
      return _context.Reviews.Where(r => r.Reviewer.Id == reviewerId).ToList();
    }

    public Reviewer GetReviewer(int reviewerId)
    {
      return _context.Reviewers.Where(r => r.Id == reviewerId).FirstOrDefault();
    }

    public ICollection<Reviewer> GetReviewers()
    {
      return _context.Reviewers.ToList();
    }

    public bool ReviewerExist(int reviewerId)
    {
      return _context.Reviewers.Any(r => r.Id == reviewerId);
    }

    public bool CreateReviewer(Reviewer reviewer)
    {
      _ = _ = _context.Add(reviewer);
      return Save();
    }

    public bool Save()
    {
      int saved = _context.SaveChanges();
      return saved > 0;
    }

    public bool UpdateReviewer(Reviewer reviewer)
    {
      _ = _context.Update(reviewer);
      return Save();
    }

    public bool DeleteReviewer(Reviewer reviewer)
    {
      _ = _context.Remove(reviewer);
      return Save();
    }
  }
}