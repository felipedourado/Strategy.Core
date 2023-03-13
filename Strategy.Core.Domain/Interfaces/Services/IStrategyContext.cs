using Strategy.Core.Domain.Models;

namespace Strategy.Core.Domain.Interfaces.Services
{
    public interface IStrategyContext
    {
        void Set(IProducts products);
        Task MethodBusiness(AccountBase request);
    }
}