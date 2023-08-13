using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WhosThatPokemonAPI.Models;

namespace WhosThatPokemonAPI.Data
{
    public class DataDbContext : DbContext
    {
        // Les options, notemment la connectionString se trouvent dans appsetttings.json et sont appelées dans DependencyInjectionExtension
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options)
        {

        }

        // DbSets
        public DbSet<Pokemon> Pokemons { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserPokemon> UserPokemons { get; set; }
        public DbSet<Models.Type> Types { get; set; }
    }
}
