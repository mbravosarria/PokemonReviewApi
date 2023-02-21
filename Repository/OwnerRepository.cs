using PokemonReviewApi.Data;
using PokemonReviewApi.Interfaces;
using PokemonReviewApi.Models;

namespace PokemonReviewApi.Repository
{
  public class OwnerRepository : IOwnerRepository
  {
    private readonly DataContext _context;

    public OwnerRepository(DataContext context)
    {
      _context = context;
    }

    public bool CreateOwner(Owner owner)
    {
      _ = _context.Add(owner);

      return Save();
    }

    public Owner GetOwner(int ownerId)
    {
      return _context.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
    }

    public ICollection<Owner> GetOwners()
    {
      return _context.Owners.ToList();
    }

    public ICollection<Owner> GetOwnersOfAPokemon(int pokemonId)
    {
      return _context.PokemonOwners.Where(po => po.PokemonId == pokemonId).Select(po => po.Owner).ToList();
    }

    public ICollection<Pokemon> GetPokemonsByOwner(int ownerId)
    {
      return _context.PokemonOwners.Where(po => po.OwnerId == ownerId).Select(po => po.Pokemon).ToList();
    }

    public bool OwnerExist(int ownerId)
    {
      return _context.Owners.Any(o => o.Id == ownerId);
    }

    public bool Save()
    {
      int saved = _context.SaveChanges();
      return saved > 0;
    }
  }
}