using PTAnalitic.Core.Interfaces.Repositories;
using System;

namespace PTAnalitic.Core.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IProductHistoryRepository ProductHistoryRepository { get; }

        int Save();
        void DetachAllEntities();
    }
}