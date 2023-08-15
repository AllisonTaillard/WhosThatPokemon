using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WhosThatPokemonAPI.Data;
using WhosThatPokemonAPI.Models;
using WhosThatPokemonAPI.Repositories;

namespace WhosThatUserAPI.Repositories
{
    public class UserRepository : IRepository<User>
    {

        private readonly DataDbContext _dbContext;

        public UserRepository(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Add(User user)
        {
            await _dbContext.Users.AddAsync(user);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(int id)
        {
            User user = await GetById(id);
            if (user == null) return false;

            _dbContext.Users.Remove(user);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<User> Get(Expression<Func<User, bool>> predicate)
        {
            return await _dbContext.Users.Include(u => u.Pokemons).FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _dbContext.Users.Include(u => u.Pokemons).ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllWhere(Expression<Func<User, bool>> predicate)
        {
            return await _dbContext.Users.Include(u => u.Pokemons).Where(predicate).ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await _dbContext.Users.Include(u => u.Pokemons).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> Update(User user)
        {
            User userFromDb = await GetById(user.Id);
            if (userFromDb == null) return false;

            // on modifie les valeurs seulement si elles différent du pokemon entré par l'utilisateur
            if (userFromDb.Pseudo != user.Pseudo) userFromDb.Pseudo = user.Pseudo;
            if (userFromDb.Level != user.Level) userFromDb.Level = user.Level;
            if (userFromDb.Xp != user.Xp) userFromDb.Xp = user.Xp;
            if (userFromDb.Password != user.Password) userFromDb.Password = user.Password;
            if (userFromDb.IsAdmin != user.IsAdmin) userFromDb.IsAdmin = user.IsAdmin;

            // Comparer également les pokemons des 2 users
            for (int i = 0; i < userFromDb.Pokemons.Count; i++)
            {
                if (userFromDb.Pokemons[i] != user.Pokemons[i]) userFromDb.Pokemons[i] = user.Pokemons[i];
            }

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddPokemonToUser(User user, UserPokemon userPokemon)
        {
            User userFromDb = await GetById(user.Id);
            if (userFromDb == null) return false;

            userFromDb.Pokemons.Add(userPokemon);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemovePokemonFromUser(User user, UserPokemon userPokemon)
        {
            User userFromDb = await GetById(user.Id);
            if (userFromDb == null) return false;

            userFromDb.Pokemons.Remove(userPokemon);

            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
