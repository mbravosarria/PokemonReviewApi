using PokemonReviewApi.Data;
using PokemonReviewApi.Interfaces;
using PokemonReviewApi.Models;

namespace PokemonReviewApi.Repository
{
  public class ReviewRepository : IReviewRepository
  {
    private readonly DataContext _context;

    public ReviewRepository(DataContext context)
    {
      _context = context;
    }

    public ICollection<Review> GetReviewsOfAPokemon(int pokemonId)
    {
      return _context.Reviews.Where(r => r.Pokemon.Id == pokemonId).ToList();
    }

    public Review GetReview(int reviewId)
    {
      return _context.Reviews.Where(r => r.Id == reviewId).FirstOrDefault();
    }

    public ICollection<Review> GetReviews()
    {
      return _context.Reviews.ToList();
    }

    public bool ReviewExist(int reviewId)
    {
      return _context.Reviews.Any(r => r.Id == reviewId);
    }

    public bool CreateReview(Review review)
    {
      _ = _context.Add(review);
      return Save();
    }

    public bool Save()
    {
      int saved = _context.SaveChanges();
      return saved > 0;
    }

    public bool UpdateReview(Review review)
    {
      _ = _context.Update(review);
      return Save();
    }

    public bool DeleteReview(Review review)
    {
      _ = _context.Remove(review);
      return Save();
    }
  }
}