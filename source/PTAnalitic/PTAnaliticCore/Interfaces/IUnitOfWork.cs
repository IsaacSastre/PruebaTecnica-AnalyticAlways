using System.Threading.Tasks;

namespace PTAnalitic.Core.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task<int> Save();
    }
}