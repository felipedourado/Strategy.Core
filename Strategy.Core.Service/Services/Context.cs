using Strategy.Core.Domain.Enum;
using Strategy.Core.Domain.Interfaces.Services;
using Strategy.Core.Domain.Models;

namespace Strategy.Core.Services
{
    public class Context : IStrategy
    {
        private IStrategy _strategy;
        //private IDal _dal; method DAL

        private readonly IEnumerable<IProducts> _products;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Context(IEnumerable<IProducts> products) => _products = products;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public void Set(IStrategy strategy)
        {
            _strategy = strategy;
        }

        public async Task Save(AccountBase request, AccountType accountType)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            await _products.FirstOrDefault(x => x.AccountType == accountType)?.Save(request);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
}