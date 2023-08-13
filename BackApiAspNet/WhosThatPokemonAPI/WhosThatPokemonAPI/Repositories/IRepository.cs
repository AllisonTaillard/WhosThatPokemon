using System.Linq.Expressions;
using WhosThatPokemonAPI.Models;

namespace WhosThatPokemonAPI.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        public Task<bool> Add(TEntity entity);
        public Task<bool> Update(TEntity entity);
        public Task<bool> Delete(int id);
        public Task<TEntity> GetById(int id);
        public Task<IEnumerable<TEntity>> GetAll();

        // predicas
        public Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
        public Task<IEnumerable<TEntity>> GetAllWhere(Expression<Func<TEntity, bool>> predicate);
    }
}
