using System.Data;
using System.Threading.Tasks;

namespace PTAnalitic.Core.Interfaces.Repositories
{
    public interface IProductHistoryRepository
    {
        bool AddRange(DataTable dt);
        bool DeleteImport();
    }
}