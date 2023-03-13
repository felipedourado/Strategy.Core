using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Strategy.Core.Models;
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

        /// <summary>
        /// Api de teste
        /// </summary>
        /// <returns></returns>
        [HttpPost("productA")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationErrorMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> SaveProductA()
        {
            _strategyContext.Set(new ProductAService());
            await _strategyContext.MethodBusiness();
            return StatusCode(201);
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