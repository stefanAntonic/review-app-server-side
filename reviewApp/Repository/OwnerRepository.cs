using reviewApp.Data;
using reviewApp.Interfaces;
using reviewApp.Models;

namespace reviewApp.Repository;

public class OwnerRepository : IOwnerRepository
{
    private readonly DataContext _context;

    public OwnerRepository(DataContext context)
    {
        _context = context;
    }
    
    public ICollection<Owner> GetOwners()
    {
        var owners = _context
            .Owners
            .OrderBy(owner => owner.Id)
            .ToList();
        return owners;
    }

    public Owner? GetOwner(int id)
    {
        Owner owner = _context
            .Owners
            .Where(owner1 => owner1.Id == id)
            .FirstOrDefault();
        return owner;
    }

    public ICollection<Owner> GetOwnerOfAPokemon(int pokeId)
    {
        ICollection<Owner>  pokemonOwner = _context
            .PokemonOwners
            .Where(owner => owner.PokemonId == pokeId)
            .Select(owner => owner.Owner)
            .ToList();
        return pokemonOwner;
    }

    public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
    {
        ICollection<Pokemon> pokemons = _context
            .PokemonOwners
            .Where(owner => owner.OwnerId == ownerId)
            .Select(pokeOwner => pokeOwner.Pokemon)
            .ToList();
        return pokemons;
    }

    public bool OwnerExisting(int id)
    {
        return _context
            .Owners
            .Any(owner => owner.Id == id);
    }

    public bool CreateOwner(Owner owner)
    {
        _context
            .Add(owner);
        return Save();    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0;
    }
}