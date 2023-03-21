using reviewApp.Data;
using reviewApp.Interfaces;
using reviewApp.Models;

namespace reviewApp.Repository;

public class PokemonRepository : IPokemonRepository
{
    private readonly DataContext _context;

    public PokemonRepository(DataContext context)
    {
        _context = context;
    }

    public ICollection<Pokemon> GetPokemons()
    {
        return _context
                .Pokemons
                .OrderBy(p => p.Id)
                .ToList();
    }
}