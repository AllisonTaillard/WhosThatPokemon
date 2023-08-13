using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WhosThatPokemonAPI.Data;
using WhosThatPokemonAPI.Models;
using Type = WhosThatPokemonAPI.Models.Type;

namespace WhosThatPokemonAPI.Repositories
{
    public class PokemonRepository : IRepository<Pokemon>
    {
        private readonly DataDbContext _dbContext;

        public PokemonRepository(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Add(Pokemon pokemon)
        {
            await _dbContext.Pokemons.AddAsync(pokemon);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(int id)
        {
            Pokemon pokemon = await GetById(id);
            if (pokemon == null) return false;

            _dbContext.Pokemons.Remove(pokemon);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<Pokemon> Get(Expression<Func<Pokemon, bool>> predicate)
        {
            return await _dbContext.Pokemons.Include(p => p.Types).FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<Pokemon>> GetAll()
        {
            return await _dbContext.Pokemons.Include(p => p.Types).ToListAsync();
        }

        public async Task<IEnumerable<Pokemon>> GetAllWhere(Expression<Func<Pokemon, bool>> predicate)
        {
            return await _dbContext.Pokemons.Include(p => p.Types).Where(predicate).ToListAsync();
        }

        public async Task<Pokemon> GetById(int id)
        {
            return await _dbContext.Pokemons.Include(p => p.Types).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> Update(Pokemon pokemon)
        {
            Pokemon pokemonFromDb = await GetById(pokemon.Id);
            if (pokemonFromDb == null) return false;

            
            // on modifie les valeurs seulement si elles différent du pokemon entré par l'utilisateur
            if (pokemonFromDb.Name != pokemon.Name) pokemonFromDb.Name = pokemon.Name;
            if (pokemonFromDb.Picture != pokemon.Picture) pokemonFromDb.Picture = pokemon.Picture;

            // Comparer également les types des 2 pokémons
            for (int i = 0; i < pokemonFromDb.Types.Count; i++)
            {
                if (pokemonFromDb.Types[i] != pokemon.Types[i]) pokemonFromDb.Types[i] = pokemon.Types[i];
            }

            // Pareil pour les users
            for (int i = 0; i < pokemonFromDb.Users.Count; i++)
            {
                if (pokemonFromDb.Users[i] != pokemon.Users[i]) pokemonFromDb.Users[i] = pokemon.Users[i];
            }

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddTypeToPokemon(Pokemon pokemon, Type type)
        {
            Pokemon pokemonFromDb = await GetById(pokemon.Id);
            if (pokemonFromDb == null) return false;

            pokemonFromDb.Types.Add(type);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveTypeFromPokemon(Pokemon pokemon, Type type)
        {
            Pokemon pokemonFromDb = await GetById(pokemon.Id);
            if (pokemonFromDb == null) return false;

            pokemonFromDb.Types.Remove(type);

            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
