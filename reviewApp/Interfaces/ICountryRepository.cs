using reviewApp.Models;

namespace reviewApp.Interfaces;

public interface ICountryRepository
{
    ICollection<Country> GetCountries();
    Country GetCountry(int id);
    Country GetCountryByOwnerId(int ownerId);
    

    bool CountryExists(int countryId);
}