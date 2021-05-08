using Microsoft.AspNetCore.Mvc;
using ValueObjects;
using Services;
using System.Threading.Tasks;

namespace ShopBridgeAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IProductsService _productsService;
		public ProductController(IProductsService productsService)
		{
			_productsService = productsService;
		}
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			return Ok(await _productsService.GetAllProductsAsync());
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			if (id <= 0)
				return BadRequest("Please specify correct Product Id");
			return Ok(await _productsService.GetProductByIdAsync(id));
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] VOProduct product)
		{
			if (product == null || string.IsNullOrWhiteSpace(product.Name) || string.IsNullOrWhiteSpace(product.Description) || product.Price == 0)
				return BadRequest("Please specify Name, Description and Price");
			return Ok(await _productsService.CreateProductAsync(product));
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Put(int id, [FromBody] VOProduct product)
		{
			if (id <= 0 || product == null || string.IsNullOrWhiteSpace(product.Name) || string.IsNullOrWhiteSpace(product.Description) || product.Price == 0)
				return BadRequest("Please specify correct details");
			return Ok(await _productsService.UpdateProductAsync(id, product));
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			if (id <= 0)
				return BadRequest("Please specify correct Product Id");
			return Ok(await _productsService.DeleteProductAsync(id));
		}
	}
}
