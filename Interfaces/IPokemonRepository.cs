using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokemonReviewApi.Models;

namespace PokemonReviewApi.Interfaces
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> GetPokemons();
        Pokemon GetPokemon(int pokemonId);
        Pokemon GetPokemon(string pokemonName);
        decimal GetPokemonRating(int pokemonId);
        bool PokemonExist(int pokemonId);
    }
}