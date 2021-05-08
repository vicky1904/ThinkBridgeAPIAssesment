using System;

namespace ShopBridgeDataModel.Transactions
{

    public interface ITransaction : IDisposable
    {
        void Commit();

        void Rollback();
    }
}
