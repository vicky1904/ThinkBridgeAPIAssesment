using ShopBridgeDataModel.Transactions;
using System;
using System.Threading.Tasks;

namespace ShopBridgeDataModel
{
    public interface IUnitofWork : IDisposable
    {
        Task<int> SaveChangesAsync();
        ITransaction BeginTransaction();
    }
}
