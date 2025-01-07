using ASPNET_WebAPI.DTOs;
using ASPNET_WebAPI.Services.UserService;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET_WebAPI.Controllers
{
    // 10 - Create a controller => root / Controllers / UserController.cs

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // 10.1 - Create private readonly fields for the services to be injected
        private readonly IUserService _userService;

        // 10.2 - Inject the services via the constructor
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // TODO: UPDATE THIS AND THE NUMERIC ORDER OF STEPS
        // 10.3 - Create the endpoints 
        // BEST PRACTICES:
        //      - Use the async/await pattern to avoid blocking the thread
        //      - ❌ Use the IActionResult interface or ActionResult<T> implementation, when multiple return types are possible
        //      and to return the correct status code and metadata, such as:
        //      Ok() 200  | CreatedAtAction() 201 | NoContent() 204 | BadRequest() 400 | NotFound() 404
        //      - ❌ GET methods should be typed with ActionResult<T> to enforce the return of specific type of data
        //      ( it is not necessary to wrap the response in Ok(), it is implicit in the success case) or return NotFound()
        //      -                                                                                                                                                                                                                                                                                                                         POST methods should return CreatedAtAction() with the created object or BadRequest()
        //      - PUT methods should return NoContent() or NotFound()
        //      - DELETE methods should return NoContent() or NotFound()

        [ProducesResponseType(typeof(IEnumerable<UserDTO>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<Ok<IEnumerable<UserDTO>>> GetAll(CancellationToken cancellationToken)
        {
            var users = await _userService.GetAllAsync(cancellationToken);
            return TypedResults.Ok(users);
        }

        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}", Name = nameof(GetById))]
        public async Task<Results<Ok<UserDTO>, NotFound<ProblemDetails>>> GetById(int id, CancellationToken cancellationToken)
        {
            var user = await _userService.GetByIdAsync(id, cancellationToken);

            if (user is null)
                return TypedResults.NotFound(new ProblemDetails
                {
                    Title = "User not found",
                    Detail = "No user found with the given Id."
                });

            return TypedResults.Ok(user);
        }

        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<Results<CreatedAtRoute<UserDTO>, BadRequest>> Create(CreateUserDTO user, CancellationToken cancellationToken)
        {
            // implicitly returns BadRequest() if the Body of the request is not valid

            var createdUser = await _userService.CreateAsync(user, cancellationToken);

            return TypedResults.CreatedAtRoute(
                createdUser,
                nameof(GetById),
                new { id = createdUser.Id }
            );
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<Results<NoContent, NotFound<ProblemDetails>, BadRequest<ProblemDetails>>> Update(int id, UpdateUserDTO user, CancellationToken cancellationToken)
        {
            // implicitly returns BadRequest() if the Body of the request is not valid

            if (id != user.Id)
                return TypedResults.BadRequest(new ProblemDetails
                {
                    Title = "Invalid request",
                    Detail = "The Id in the route does not match the Id in the request body."
                });

            // Not sure if this check should be here, or in the service
            var existingUser = await _userService.GetByIdAsync(id, cancellationToken);

            if (existingUser is null)
                return TypedResults.NotFound(new ProblemDetails
                {
                    Title = "User not found",
                    Detail = "No user found with the given Id."
                }); ;

            await _userService.UpdateAsync(id, user, cancellationToken);

            return TypedResults.NoContent();
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<Results<NoContent, NotFound<ProblemDetails>, BadRequest>> Delete(int id, CancellationToken cancellationToken)
        {
            // Not sure if this check should be here, or in the service
            var existingUser = await _userService.GetByIdAsync(id, cancellationToken);

            if (existingUser is null)
                return TypedResults.NotFound(new ProblemDetails
                {
                    Title = "Invalid request",
                    Detail = "The Id in the route does not match the Id in the request body."
                });

            await _userService.DeleteAsync(id, cancellationToken);

            return TypedResults.NoContent();
        }
    }
}
