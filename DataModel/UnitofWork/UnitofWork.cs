using ShopBridgeDataModel.Transactions;
using System;
using System.Threading.Tasks;

namespace ShopBridgeDataModel
{
    public class UnitofWork : IUnitofWork
    {
        private readonly DataContext _context = null;
        private bool disposed;

        public UnitofWork(DataContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public ITransaction BeginTransaction()
        {
            return new EntityDatabaseTransaction(_context);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                _context.Dispose();
            }
            disposed = true;
        }
    }
}
