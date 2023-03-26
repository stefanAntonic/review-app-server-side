using reviewApp.Data;
using reviewApp.Interfaces;
using reviewApp.Models;

namespace reviewApp.Repository;

public class CountryRepository : ICountryRepository
{
    private readonly DataContext _context;

    public CountryRepository(DataContext context)
    {
        _context = context;
    }

    public ICollection<Country> GetCountries()
    {
        var countries = _context
            .Countries
            .OrderBy(country => country.Id)
            .ToList();
        return countries;
    }

    public Country GetCountry(int id)
    {
        // ReSharper disable once ReplaceWithSingleCallToFirstOrDefault
        return _context
            .Countries
            .Where(country => country.Id == id)
            .FirstOrDefault()!;
        }

    public Country GetCountryByOwnerId(int ownerId)
    {
        // ReSharper disable once ReplaceWithSingleCallToFirstOrDefault
        var country = _context
            .Countries
            .Where(country1 => country1.Owners.Any(owner => owner.Id == ownerId))
            .FirstOrDefault();
        return country;
    }
    

    public bool CountryExists(int countryId)
    {
        return _context.Countries.Any(country => country.Id == countryId);
    }
}