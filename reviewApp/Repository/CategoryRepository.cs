using Microsoft.EntityFrameworkCore;
using reviewApp.Data;
using reviewApp.Interfaces;
using reviewApp.Models;

namespace reviewApp.Repository;

public class CategoryRepository : ICategoryRepository
{
    private readonly DataContext _context;

    public CategoryRepository(DataContext context)
    {
        _context = context;
    }
    
    public ICollection<Category> GetCategories()
    {
        var categories = _context
            .Categories
            .OrderBy(category => category.Id)
            .ToList();
        return categories;
    }

    public Category? GetCategory(int id)
    {
        // ReSharper disable once ReplaceWithSingleCallToFirstOrDefault
        var category = _context
            .Categories
            .Where(category => category.Id == id)
            .FirstOrDefault();
        return category;
    }

    public ICollection<Pokemon> GetPokemonByCategory(int categoryId)
    {
        var pokemons = _context
            .Pokemons
            .Where(pokemon1 => pokemon1.PokemonCategories
                .Any(category => category.CategoryId == categoryId))
            .ToList();
        return pokemons;
    }

    public bool CategoryExists(int id)
    {
        return _context.Categories.Any(category => category.Id == id);
    }

    public bool CreateCategory(Category category)
    {
        //Change tracker 
        //Add, update, modifying
        //connected vs disconnected 
        _context
            .Add(category);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0;
    }
    
}