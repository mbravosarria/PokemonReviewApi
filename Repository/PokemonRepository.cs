using PokemonReviewApi.Data;
using PokemonReviewApi.Interfaces;
using PokemonReviewApi.Models;

namespace PokemonReviewApi.Repository
{
  public class PokemonRepository : IPokemonRepository
  {
    private readonly DataContext _context;

    public PokemonRepository(DataContext context)
    {
      _context = context;
    }

    public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
    {
      Owner owner = _context.Owners.Where(o => o.Id == ownerId)
        .FirstOrDefault();
      Category category = _context.Categories.Where(c => c.Id == categoryId)
        .FirstOrDefault();

      PokemonOwner pokemonOwner = new()
      {
        Owner = owner,
        Pokemon = pokemon,
      };

      _ = _context.Add(pokemonOwner);

      PokemonCategory pokemonCategory = new()
      {
        Category = category,
        Pokemon = pokemon,
      };

      _ = _context.Add(pokemonCategory);

      _ = _context.Add(pokemon);

      return Save();
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
      IQueryable<Review> review = _context.Reviews.Where(r => r.Pokemon.Id == pokemonId);

      return !review.Any() ? 0 : review.Sum(r => r.Rating) / review.Count();
    }

    public ICollection<Pokemon> GetPokemons()
    {
      return _context.Pokemons.OrderBy(p => p.Id).ToList();
    }

    public bool PokemonExist(int pokemonId)
    {
      return _context.Pokemons.Any(p => p.Id == pokemonId);
    }

    public bool Save()
    {
      int saved = _context.SaveChanges();
      return saved > 0;
    }
  }
}