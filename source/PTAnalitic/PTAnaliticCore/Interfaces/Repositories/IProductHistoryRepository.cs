using PTAnalitic.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PTAnalitic.Core.Interfaces.Repositories
{
    public interface IProductHistoryRepository
    {
        Task<bool> AddRange(IList<ProductHistory> productHistories);
        Task<bool> DeleteImport();
    }
}