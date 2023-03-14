using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Strategy.Core.Domain.Enum;
using Strategy.Core.Domain.Interfaces.Services;
using Strategy.Core.Domain.Models;
using Strategy.Core.Models;
using Strategy.Core.Services;
using System.Threading.Tasks;

namespace Strategy.Core.Controllers
{
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IStrategy _strategy;

        public ProductsController(IStrategy strategy)
        {
            _strategy = strategy;
        }

        /// <summary>
        /// Api save digital account in mongodb
        /// </summary>
        /// <returns></returns>
        [HttpPost("digital-account")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationErrorMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> SaveDigitalAccount(DigitalAccountRequest request)
        {
            await _strategy.Save(request, AccountType.Digital);
            return StatusCode(201);
        }

        /// <summary>
        /// Api save physical account sql server 
        /// </summary>
        /// <returns></returns>
        [HttpPost("physical-account")]
        public async Task<ActionResult> SavePhysicalAccount(PhysicalAccountRequest request)
        {
            await _strategy.Save(request, AccountType.Physical);
            return StatusCode(201);
        }
    }
}