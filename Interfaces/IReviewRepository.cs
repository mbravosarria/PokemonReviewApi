using PokemonReviewApi.Models;

namespace PokemonReviewApi.Interfaces
{
  public interface IReviewRepository
  {
    ICollection<Review> GetReviews();
    Review GetReview(int reviewId);
    ICollection<Review> GetReviewsOfAPokemon(int pokemonId);
    bool ReviewExist(int reviewId);
    bool CreateReview(Review review);
    bool Save();
  }
}