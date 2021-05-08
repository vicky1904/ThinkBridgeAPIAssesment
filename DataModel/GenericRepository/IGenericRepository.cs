using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopBridgeDataModel
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIDAsync(object id);
        Task InsertAsync(TEntity entity);
        void Delete(TEntity entityToDelete);
        void Update(TEntity entityToUpdate);
        Task<IEnumerable<TEntity>> GetAllAsync();
    }
}
