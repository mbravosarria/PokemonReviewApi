using PokemonReviewApi.Models;

namespace PokemonReviewApi.Interfaces
{
  public interface IOwnerRepository
  {
    ICollection<Owner> GetOwners();
    Owner GetOwner(int ownerId);
    ICollection<Owner> GetOwnersOfAPokemon(int pokemonId);
    ICollection<Pokemon> GetPokemonsByOwner(int ownerId);
    bool OwnerExist(int ownerId);
    bool CreateOwner(Owner owner);
    bool Save();
  }
}