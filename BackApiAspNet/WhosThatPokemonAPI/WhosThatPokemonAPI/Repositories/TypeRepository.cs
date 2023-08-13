using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using WhosThatPokemonAPI.Data;
using WhosThatPokemonAPI.Models;
using WhosThatPokemonAPI.Repositories;
using Type = WhosThatPokemonAPI.Models.Type;

namespace WhosThatPokemonAPI.Repositories
{
    public class TypeRepository : IRepository<Type>
    {
        private readonly DataDbContext _dbContext;

        public TypeRepository(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Add(Type type)
        {
            await _dbContext.Types.AddAsync(type);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(int id)
        {
            Type type = await GetById(id);
            if (type == null) return false;

            _dbContext.Types.Remove(type);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<Type> Get(Expression<Func<Type, bool>> predicate)
        {
            return await _dbContext.Types.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<Type>> GetAll()
        {
            return await _dbContext.Types.ToListAsync();
        }

        public async Task<IEnumerable<Type>> GetAllWhere(Expression<Func<Type, bool>> predicate)
        {
            return await _dbContext.Types.Where(predicate).ToListAsync();
        }

        public async Task<Type> GetById(int id)
        {
            return await _dbContext.Types.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<bool> Update(Type type)
        {
            Type typeFromDb = await GetById(type.Id);
            if (typeFromDb == null) return false;

            if (typeFromDb.Name != type.Name) typeFromDb.Name = type.Name;

            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
