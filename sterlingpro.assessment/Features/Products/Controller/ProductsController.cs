using MediatR;
using Microsoft.AspNetCore.Mvc;
using sterlingpro.assessment.Features.Cart.AddToCart;
using sterlingpro.assessment.Features.Cart.GetCart;
using sterlingpro.assessment.Features.Cart.RemoveFromCart;
using sterlingpro.assessment.Features.Cart.UpdateCartItem;
using sterlingpro.assessment.Features.Products.CreateProduct;
using sterlingpro.assessment.Features.Products.GetProducts;
using sterlingpro.assessment.Features.Products.GetProductsById;

namespace sterlingpro.assessment.Features.Products.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] GetProductsQuery query)
        {
            var products = await _mediator.Send(query);
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
        {
            try
            {
                var product = await _mediator.Send(command);
                return CreatedAtAction(nameof(CreateProduct), new { id = product.Id }, product);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var query = new GetProductByIdQuery { Id = id };
                var product = await _mediator.Send(query);
                return Ok(product);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }  
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}

