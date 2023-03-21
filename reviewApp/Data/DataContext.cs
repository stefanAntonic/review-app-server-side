using Microsoft.EntityFrameworkCore;
using reviewApp.Models;

namespace reviewApp.Data;
// U ovoj klasi se vrsi podesavanje baze
// U terminalu za migraciju je potrebno izvrsiti sljedece komande 
//  dotnet ef migrations add "initialSetup" u rideru 
// ili Add-Migration InitialCreate u Package Manager console u visual studio 
//Nakon toga ide komanda Update-Database
public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }
    //Kreiranje tabela u bazi od modela
    public DbSet<Category> Categories { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Owner> Owners { get; set; }
    public DbSet<Pokemon> Pokemons { get; set; }
    public DbSet<PokemonOwner> PokemonOwners { get; set; }
    public DbSet<PokemonCategory> PokemonCategories { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Reviewer> Reviwers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Kreiranje relacija u bazi koristeci entity framework 
        // Entity moze da koristi linq za pisanje kverija u bazu
       // Kreiranje relacije PokemonCategory 
       modelBuilder.Entity<PokemonCategory>()
           .HasKey(pc => new { pc.PokemonId, pc.CategoryId });
       modelBuilder.Entity<PokemonCategory>()
           .HasOne(p => p.Pokemon)
           .WithMany(pc => pc.PokemonCategories)
           .HasForeignKey(p => p.PokemonId);
       modelBuilder.Entity<PokemonCategory>()
           .HasOne(p => p.Category)
           .WithMany(pc => pc.PokemonCategories)
           .HasForeignKey(c => c.CategoryId);
        
        // Kreiranje relacije za PokemonOwner 
        modelBuilder.Entity<PokemonOwner>()
            .HasKey(po => new { po.PokemonId, po.OwnerId });
        modelBuilder.Entity<PokemonOwner>()
            .HasOne(p => p.Pokemon)
            .WithMany(pc => pc.PokemonOwners)
            .HasForeignKey(p => p.PokemonId);
        modelBuilder.Entity<PokemonOwner>()
            .HasOne(p => p.Owner)
            .WithMany(pc => pc.PokemonOwners)
            .HasForeignKey(c => c.OwnerId);
    }
}