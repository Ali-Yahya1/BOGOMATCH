using BOGOMATCH_DOMAIN.INTERFACE;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BOGOMATCH.Controllers.V1.AccountController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProducts _IProducts;
        public ProductController(IProducts IProducts)
        {
            _IProducts = IProducts;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            return Ok(await _IProducts.GetAllProducts());
        }
    }
}
