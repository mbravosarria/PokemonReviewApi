using PokemonReviewApi.Data;
using PokemonReviewApi.Interfaces;
using PokemonReviewApi.Models;

namespace PokemonReviewApi.Repository
{
  public class CountryRepository : ICountryRepository
  {
    private readonly DataContext _context;

    public CountryRepository(DataContext context)
    {
      _context = context;
    }

    public bool CountryExist(int countryId)
    {
      return _context.Countries.Any(c => c.Id == countryId);
    }

    public bool CreateCountry(Country country)
    {
      _ = _context.Add(country);

      return Save();
    }

    public bool DeleteCountry(Country country)
    {
      _ = _context.Remove(country);
      return Save();
    }

    public ICollection<Country> GetCountries()
    {
      return _context.Countries.ToList();
    }

    public Country GetCountry(int countryId)
    {
      return _context.Countries.Where(c => c.Id == countryId).FirstOrDefault();
    }

    public Country GetCountryByOwner(int ownerId)
    {
      return _context.Owners.Where(o => o.Id == ownerId).Select(o => o.Country).FirstOrDefault();
    }

    public ICollection<Owner> GetOwnersFromACountry(int countryId)
    {
      return _context.Owners.Where(o => o.Country.Id == countryId).ToList();
    }

    public bool Save()
    {
      int saved = _context.SaveChanges();
      return saved > 0;
    }

    public bool UpdateCountry(Country country)
    {
      _ = _context.Update(country);
      return Save();
    }
  }
}