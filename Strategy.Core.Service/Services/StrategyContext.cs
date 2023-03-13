using Strategy.Core.Domain.Interfaces.Services;
using Strategy.Core.Domain.Models;

namespace Strategy.Core.Services
{
    public class StrategyContext : IStrategyContext
    {
        private IProducts _products;
        //private IDal _dal; method DAL

        public StrategyContext()
        {
        }

        public StrategyContext(IProducts products)
        {
            _products = products;
        }

        public void Set(IProducts products)
        {
            _products = products;
        }

        public async Task MethodBusiness(AccountBase request)
        {
            //Call method by product context
            _products.Save(request);
            
        }
    }
}