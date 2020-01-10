using System.Threading.Tasks;

namespace PTAnalitic.Core.Interfaces.Services
{
    public interface IProductHistoryService
    {
        Task<bool> ImportDataFromAzure();
    }
}