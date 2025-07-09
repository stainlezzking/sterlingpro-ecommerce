namespace sterlingpro.assessment.Features.Cart.Controller
{
    using global::sterlingpro.assessment.Features.Cart.AddToCart;
    using global::sterlingpro.assessment.Features.Cart.GetCart;
    using global::sterlingpro.assessment.Features.Cart.RemoveFromCart;
    using global::sterlingpro.assessment.Features.Cart.UpdateCartItem;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;


    namespace sterlingpro.assessment.Features.Products.Controller
    {
        [ApiController]
        [Authorize]
        [Route("api/[controller]")]
        public class CartController : ControllerBase
        {
            private readonly IMediator _mediator;

            public CartController(IMediator mediator)
            {
                _mediator = mediator;
            }


            [HttpPost]
            public async Task<IActionResult> AddToCart([FromBody] AddToCartCommand command)
            {
                try
                {
                    var result = await _mediator.Send(command);
                    return Ok(new { message = result });
                }
                catch (ArgumentException ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
                catch (InvalidOperationException ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }

            [HttpGet]
            public async Task<IActionResult> GetCart()
            {
                var query = new GetCartQuery();
                var result = await _mediator.Send(query);
                return Ok(result);
            }

            [HttpPut]
            public async Task<IActionResult> UpdateCartItem([FromBody] UpdateCartItemCommand command)
            {
                try
                {
                    var result = await _mediator.Send(command);
                    return Ok(new { message = result });
                }
                catch (ArgumentException ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
                catch (InvalidOperationException ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }

            [HttpDelete("{cartItemId}")]
            public async Task<IActionResult> RemoveFromCart(int cartItemId)
            {
                try
                {
                    var command = new RemoveFromCartCommand { CartItemId = cartItemId };
                    var result = await _mediator.Send(command);
                    return Ok(new { message = result });
                }
                catch (ArgumentException ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }
        }
    }


}
