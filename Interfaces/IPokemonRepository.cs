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
    bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon);
    bool UpdatePokemon(Pokemon pokemon);
    bool DeletePokemon(Pokemon pokemon);
    bool Save();
  }
}