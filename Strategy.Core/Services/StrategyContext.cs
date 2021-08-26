using System.Threading.Tasks;
using Strategy.Core.Services.Interfaces;

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

        public async Task MethodBusiness()
        {
            //Call method by product context
            
        }
    }
}