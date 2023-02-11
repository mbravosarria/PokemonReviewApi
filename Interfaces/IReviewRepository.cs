using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokemonReviewApi.Models;

namespace PokemonReviewApi.Interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int reviewId);
        ICollection<Review> GetReviewsOfAPokemon(int pokemonId);
        bool ReviewExist(int reviewId);
    }
}