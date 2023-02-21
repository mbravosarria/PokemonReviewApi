using PokemonReviewApi.Data;
using PokemonReviewApi.Interfaces;
using PokemonReviewApi.Models;

namespace PokemonReviewApi.Repository
{
  public class CategoryRepository : ICategoryRepository
  {
    private readonly DataContext _context;

    public CategoryRepository(DataContext context)
    {
      _context = context;
    }
    public bool CategoryExist(int categoryId)
    {
      return _context.Categories.Any(c => c.Id == categoryId);
    }

    public bool CreateCategory(Category category)
    {
      _ = _context.Add(category);

      return Save();
    }

    public ICollection<Category> GetCategories()
    {
      return _context.Categories.ToList();
    }

    public Category GetCategory(int categoryId)
    {
      return _context.Categories.Where(c => c.Id == categoryId).FirstOrDefault();
    }

    public ICollection<Pokemon> GetPokemonsByCategory(int categoryId)
    {
      return _context.PokemonCategories.Where(c => c.CategoryId == categoryId).Select(c => c.Pokemon).ToList();
    }

    public bool Save()
    {
      int saved = _context.SaveChanges();
      return saved > 0;
    }
  }
}