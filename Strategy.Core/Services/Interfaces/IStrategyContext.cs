using System.Threading.Tasks;

namespace Strategy.Core.Services.Interfaces
{
    public interface IStrategyContext
    {
        void Set(IProducts products);
        Task MethodBusiness();
    }
}