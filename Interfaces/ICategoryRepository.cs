using PokemonReviewApi.Models;

namespace PokemonReviewApi.Interfaces
{
  public interface ICategoryRepository
  {
    ICollection<Category> GetCategories();
    Category GetCategory(int categoryId);
    ICollection<Pokemon> GetPokemonsByCategory(int categoryId);
    bool CategoryExist(int categoryId);
    bool CreateCategory(Category category);
    bool UpdateCategory(Category category);
    bool DeleteCategory(Category category);
    bool Save();
  }
}