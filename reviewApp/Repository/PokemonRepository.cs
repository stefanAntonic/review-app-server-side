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

    public Pokemon GetPokemon(int id)
    {
        // ReSharper disable once ReplaceWithSingleCallToFirstOrDefault
        return _context
            .Pokemons
            .Where(p => p.Id == id)
            .FirstOrDefault()!;
    }

    public Pokemon GetPokemon(string name)
    {
        // ReSharper disable once ReplaceWithSingleCallToFirstOrDefault
        return _context
            .Pokemons
            .Where(p => p.Name == name)
            .FirstOrDefault()!;
    }

    public decimal GetPokemonRating(int pokeId)
    {
        var review = _context
            .Reviews
            .Where(r => r.Pokemon.Id == pokeId);
        if (review.Count() <= 0)
        {
            return 0;
        }

        return ((decimal)review.Sum(r => r.Rating) / review.Count());
    }

    public bool PokemonExists(int pokeId)
    {
        return _context.Pokemons.Any(p => p.Id == pokeId);
    }
}