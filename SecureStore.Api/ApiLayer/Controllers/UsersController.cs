using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureStore.Api.ApplicationLayer.Commands.Users;
using SecureStore.Api.ApplicationLayer.Queries.Users;

namespace SecureStore.Api.ApiLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;


        public UsersController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("Register")]
        async public Task<IActionResult> UserRegistration([FromBody] CreateUserCommand request)
        {

            var Token = await _mediator.Send(request);

            return Ok(Token);

        }

        [HttpGet]
        [Route("Login")]
        [AllowAnonymous]
        async public Task<IActionResult> UserLogin([FromQuery]UserLoginQuery  request)
        {
            var Token = await _mediator?.Send(request);

            return Ok(Token);
        }
        
        [HttpPut]
        [Route("Edit")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UserEdit([FromBody]EditUserCommand editUserCommand)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

            var IsValid = int.TryParse(userIdClaim.Value, out var userId);

            if (!IsValid) return Unauthorized();

            editUserCommand.UserId = userId;

            var NewUser = await _mediator.Send(editUserCommand);

            return Ok(NewUser);

        }
    }
}
