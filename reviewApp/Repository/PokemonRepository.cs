﻿using reviewApp.Data;
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

    
    public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
    {
        var pokemonOwnerEntity = _context.Owners.Where(owner => owner.Id == ownerId).FirstOrDefault();
        var pokemonCategoryEntity = _context.Categories.Where(category => category.Id == categoryId).FirstOrDefault();

        var pokemonOwner = new PokemonOwner()
        {
            Owner = pokemonOwnerEntity,
            Pokemon = pokemon,
        };
        


        var pokemonCategory = new PokemonCategory()
        {
            Category = pokemonCategoryEntity,
            Pokemon = pokemon,
        };
        _context.Add(pokemonOwner);
        _context.Add(pokemonCategory);
        return Save();
        
    }

    public bool UpDatePokemon(Pokemon pokemon)
    {
        _context.Update(pokemon);
        return Save();
    }

    public bool DeletePokemon(Pokemon pokemon)
    {
        _context.Remove(pokemon);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0;
    }
}