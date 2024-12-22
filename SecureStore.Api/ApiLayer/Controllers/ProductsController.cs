using MediatR;
using Microsoft.AspNetCore.Mvc;
using SecureStore.Api.ApplicationLayer.Commands.Products;
using SecureStore.Api.ApplicationLayer.Queries.Products;

namespace SecureStore.Api.ApiLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("{ProductId}")]
        public async Task<IActionResult> GetPorductById([FromRoute] int ProductId)
        {
            var ProductQuery = new GetProductByIdQuery() { ProductId = ProductId };

            var ProductDTO = await _mediator.Send(ProductQuery);

            if (ProductDTO == null)
            {
                return BadRequest();
            }
            return Ok(ProductDTO);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery]GetAllProductsQuery getAllProducts)
        {
            var AllProducts = await _mediator.Send(getAllProducts);

            return Ok(AllProducts);
        }

        [HttpGet("Page/{Page}")]
        public async Task<IActionResult> GetProductsByPage([FromRoute]int Page)
        {
            int ProductCountInPage = 2;
            var GetProductsInPageQuery = new GetProductsByPageAndPageSizeQuery() { ProductCountInPage = ProductCountInPage, Page = Page };
            var ProductsInPageDTO = await _mediator.Send(GetProductsInPageQuery);

            if (ProductsInPageDTO == null)
            {
                return BadRequest();
            }
            return Ok(ProductsInPageDTO);
        }

        [HttpGet("Category/{Category}")]
        public async Task<IActionResult> GetProductsByCategory([FromRoute]string Category)
        {
            var GetProductsByCategoryQuery = new GetProductsByCategoryQuery() { Categry = Category };
            var Products = await _mediator.Send(GetProductsByCategoryQuery);

            if(Products == null)
                return BadRequest();
            return Ok(Products);
        }

        [HttpDelete("Delete/{ProductName}")]
        public async Task<IActionResult> DeleteProductByProductName([FromRoute] string ProductName)
        {
            var DeleteProductByNameCommand = new DeleteProductByProductNameCommand() { ProductName = ProductName };

            var IsDelete = await _mediator.Send(DeleteProductByNameCommand);

            if (IsDelete) return Ok();

            return BadRequest();
        }
    }
}
