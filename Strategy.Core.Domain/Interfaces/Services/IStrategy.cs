using Strategy.Core.Domain.Enum;
using Strategy.Core.Domain.Models;

namespace Strategy.Core.Domain.Interfaces.Services
{
    public interface IStrategy
    {
        Task Save(AccountBase request, AccountType accountType);
    }
}