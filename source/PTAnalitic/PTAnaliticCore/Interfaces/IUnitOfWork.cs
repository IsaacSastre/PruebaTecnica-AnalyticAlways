using PTAnalitic.Core.Interfaces.Repositories;
using System.Threading.Tasks;

namespace PTAnalitic.Core.UnitOfWork
{
    public interface IUnitOfWork
    {
        IProductHistoryRepository ProductHistoryRepository { get; }

        Task<int> Save();
    }
}