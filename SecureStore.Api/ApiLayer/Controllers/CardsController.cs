using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureStore.Api.ApplicationLayer.Commands.ShoppingCarts;
using SecureStore.Api.ApplicationLayer.Queries.ShoppingCarts;
using SecureStore.Api.ApplicationLayer.Requests.ShoppingCartRequestModel;

namespace SecureStore.Api.ApiLayer.Controllers
{
    [ApiController]
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    public class CardsController : ControllerBase
    {
        IMediator _mediator;

        public CardsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("add-to-card")]

        public async Task<IActionResult> AddToCardAsync([FromQuery] AddToShoppingCartRequestModel request)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

            var IsValid = int.TryParse(userIdClaim.Value, out var userId);

            if (!IsValid) return Unauthorized();

            AddItemToShoppingCartCommand AddItemCommand = new AddItemToShoppingCartCommand()
            {
                productId = request.ProductId,
                quantity = request.Quantity,
                UserId = userId
            };


            var ShoppingCartDTO = await _mediator?.Send(AddItemCommand);

            return Ok(ShoppingCartDTO);

        }

        [HttpPost]
        [Route("CheckingOrder")]
        public async Task<IActionResult> CheckingOrderAsync()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

            var IsValid = int.TryParse(userIdClaim.Value, out var userId);

            if (!IsValid) return Unauthorized();

            CheckingOrderCommand checkingOrder = new CheckingOrderCommand() { UserId = userId };

            var OrderDTO = await _mediator.Send(checkingOrder);

            return Ok(OrderDTO);

        }

        [HttpDelete]
        [Route("Clear-Card")]
        public async Task<IActionResult> ClearCartAsync()
        {
            var UserClaim = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

            var IsValid = int.TryParse(UserClaim.Value, out var userId);

            if (!IsValid) return Unauthorized();

            var ClearCardCommand = new ClearShoppingCartByUserIdCommand() { UserId = userId };

            var ClearCart = await _mediator.Send(ClearCardCommand);

            return Ok(ClearCart);
        }

        [HttpGet]
        [Route("User")]
        public async Task<IActionResult> GetCartByUserIdAsync()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

            var Isvalid = int.TryParse(userIdClaim.Value,out var userId);

            if (!Isvalid) return Unauthorized();

            var GetByIdCommand = new GetShoppingCartByUserIdQuery() { UserId = userId };

            var ShoppingCartDTO = await _mediator.Send(GetByIdCommand);

            return ShoppingCartDTO == null ? NotFound() : Ok(ShoppingCartDTO);
        }



    }
}
