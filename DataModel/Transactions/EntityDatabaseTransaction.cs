using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ShopBridgeDataModel.Transactions
{
    public class EntityDatabaseTransaction : ITransaction
    {
        private readonly IDbContextTransaction transaction;
        private bool disposed;

        public EntityDatabaseTransaction(DbContext context)
        {
            transaction = context.Database.BeginTransaction();
        }

        public void Commit()
        {
            transaction.Commit();
        }

        public void Rollback()
        {
            transaction.Rollback();
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
                transaction.Dispose();
            }
            disposed = true;
        }
    }
}
