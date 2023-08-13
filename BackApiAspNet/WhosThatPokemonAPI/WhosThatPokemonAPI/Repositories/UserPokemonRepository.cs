using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using WhosThatPokemonAPI.Data;
using WhosThatPokemonAPI.Models;

namespace WhosThatPokemonAPI.Repositories
{
    public class UserPokemonRepository : IRepository<UserPokemon>
    {
        private readonly DataDbContext _dbContext;

        public UserPokemonRepository(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Add(UserPokemon userPokemon)
        {
            await _dbContext.UserPokemons.AddAsync(userPokemon);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(int id)
        {
            UserPokemon userPokemon = await GetById(id);
            if (userPokemon == null) return false;

            _dbContext.UserPokemons.Remove(userPokemon);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<UserPokemon> Get(Expression<Func<UserPokemon, bool>> predicate)
        {
            return await _dbContext.UserPokemons.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<UserPokemon>> GetAll()
        {
            return await _dbContext.UserPokemons.ToListAsync();
        }

        public async Task<IEnumerable<UserPokemon>> GetAllWhere(Expression<Func<UserPokemon, bool>> predicate)
        {
            return await _dbContext.UserPokemons.Where(predicate).ToListAsync();
        }

        public async Task<UserPokemon> GetById(int id)
        {
            return await _dbContext.UserPokemons.FirstOrDefaultAsync(u => u.Id == id);
        }

        public Task<bool> Update(UserPokemon userPokemon)
        {
            throw new NotImplementedException();
        }
    }
}
