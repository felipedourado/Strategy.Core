using Strategy.Core.Domain.Models;

namespace Strategy.Core.Domain.Interfaces.Services
{
    public interface IProducts
    {
        void Save(AccountBase request);
    }
}