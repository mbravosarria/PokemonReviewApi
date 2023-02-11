using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokemonReviewApi.Data;
using PokemonReviewApi.Interfaces;
using PokemonReviewApi.Models;

namespace PokemonReviewApi.Repository
{
    public class PokemonRepository: IPokemonRepository
    {
        private readonly DataContext _context;

        public PokemonRepository(DataContext context)
        {
            _context = context;
        }

        public Pokemon GetPokemon(int pokemonId)
        {
            return _context.Pokemons.Where(p => p.Id == pokemonId).FirstOrDefault();
        }

        public Pokemon GetPokemon(string pokemonName)
        {
            return _context.Pokemons.Where(p => p.Name == pokemonName).FirstOrDefault();
        }

        public decimal GetPokemonRating(int pokemonId)
        {
            var review = _context.Reviews.Where(r => r.Pokemon.Id == pokemonId);

            if (!review.Any())
                return 0;

            return (decimal)review.Sum(r => r.Rating) / review.Count();
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _context.Pokemons.OrderBy(p => p.Id).ToList();
        }

        public bool PokemonExist(int pokemonId)
        {
            return _context.Pokemons.Any(p=> p.Id == pokemonId);
        }
    }
}