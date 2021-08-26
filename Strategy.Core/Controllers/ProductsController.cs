using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Strategy.Core.Services;
using Strategy.Core.Services.Interfaces;

namespace Strategy.Core.Controllers
{
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IStrategyContext _strategyContext;

        public ProductsController(IStrategyContext strategyContext)
        {
            this._strategyContext = strategyContext;
        }

        [HttpPost("productA")]
        public async Task<ActionResult> SaveProductA()
        {
            _strategyContext.Set(new ProductAService());
            await _strategyContext.MethodBusiness();
            return Ok();
        }

        [HttpPost("productB")]
        public async Task<ActionResult> SaveProductB()
        {
            _strategyContext.Set(new ProductBService());
            await _strategyContext.MethodBusiness();
            return Ok();
        }
    }
}