using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureStore.Api.ApplicationLayer.Commands.Products;
using SecureStore.Api.ApplicationLayer.Commands.Users;
using SecureStore.Api.InfrastructureLayer.Repositories.Repositories;

namespace SecureStore.Api.ApiLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private IUserRepository _userRepository;
        private IProductRepository _productRepository;
        private ICartItemRepository _cartItemRepository;
        private IMediator _mediator;

        public AdminController(
            IUserRepository userRepository,
            IProductRepository productRepository,
            ICartItemRepository cartItemRepository,
            IMediator mediator)
        {
            _userRepository = userRepository;
            _productRepository = productRepository;
            _cartItemRepository = cartItemRepository;
            _mediator = mediator;
        }

        [HttpDelete("DeleteUser/{UserName}")]
        public async Task<IActionResult> DeleteUser(string UserName)
        {
            var DeleteUserCommand = new DeleteUserByUserNameCommand() { UserName = UserName };

            if (await _mediator.Send(DeleteUserCommand))
                return Ok();

            return BadRequest();
        }


        [HttpDelete("DeleteProduct/{ProrudctName}")]
        public async Task<IActionResult> DeleteProduct(string ProrudctName)
        {
            var DeleteCommand = new DeleteProductByProductNameCommand() { ProductName = ProrudctName };

            if (await _mediator.Send(DeleteCommand))
                return Ok();

            return BadRequest();
        }

        [HttpPut("UpdateProductQuantity/{ProductId}/{ProductQuantity}")]
        public async Task<IActionResult> UpdateProductQuantity(int ProductId, int ProductQuantity)
        {

            var UpdateCommand = new UpdateProductQuantityCommand() { productId = ProductId, Quantity = ProductQuantity };
            
            var ProductDTO = await _mediator.Send(UpdateCommand);

            if(ProductDTO != null)
                return Ok(ProductDTO);

            return BadRequest();
        }

        [HttpPut("PassRoleIntoUser/{UserName}")]
        public async Task<IActionResult> ChangeAdminRoleIntoUser(string UserName)
        {
            var AdminIdClaim = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");


            var IsValid = int.TryParse(AdminIdClaim.Value, out var AdminId);


            if (!IsValid) return Unauthorized();

            var UpdateRoleCommandForAdmin = new UpdateUserRoleByUserNameCommand() { UserName = UserName};

           if(await _mediator.Send(UpdateRoleCommandForAdmin))
            {
                return Ok();
            }
           return BadRequest();
        }

    }
}
