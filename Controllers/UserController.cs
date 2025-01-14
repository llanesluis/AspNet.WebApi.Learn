using ASPNET_WebAPI.DTOs;
using ASPNET_WebAPI.Services.UserService;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET_WebAPI.Controllers
{
    // 12 - Create a Controller => root / Controllers / UserController.cs

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // 12.1 - Create private readonly fields for the services to be injected
        private readonly IUserService _userService;

        // 12.2 - Inject the services via the constructor
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // 12.3 - Create the endpoints 
        // CONSIDERATIONS:
        //      - Use the async/await pattern to avoid blocking the thread
        //      - Use DTOs to accept data via request body and return DTOs to the client (if DTOs are implemented)

        //      - The IActionResult interface or ActionResult<T> implementation, allow multiple return types
        //          BAD: it does not guarantee Type Safety in compile time 
        //          GOOD: returns the correct status code and metadata automatically used by Swagger, such as:
        //                Ok() 200  | CreatedAtAction() 201 | NoContent() 204 | BadRequest() 400 | NotFound() 404

        //      - The Results<T1<T3>, T2> only allows the specific types to be returned
        //          BAD: It does not automatically provide metadata for Swagger, for that Response Types need to be annotated
        //                [ProducesResponseType(typeof(IEnumerable<UserDTO>), StatusCodes.Status200OK)]
        //          GOOD: Provides Type Safety and only the specified return types are allowed

        //      - A custom Wrapper can be implemented to provide a contract for the return types

        [HttpGet]
        public async Task<Ok<IEnumerable<UserDTO>>> GetAll(CancellationToken cancellationToken)
        {
            var users = await _userService.GetAllAsync(cancellationToken);
            return TypedResults.Ok(users);
        }

        [HttpGet("{id}", Name = nameof(GetById))]
        public async Task<Results<Ok<UserDTO>, NotFound<ProblemDetails>>> GetById(int id, CancellationToken cancellationToken)
        {
            var user = await _userService.GetByIdAsync(id, cancellationToken);

            if (user is null)
            {
                return TypedResults.NotFound(new ProblemDetails
                {
                    Title = "User not found",
                    Detail = "No user found with the given Id."
                });
            }

            return TypedResults.Ok(user);
        }

        [HttpPost]
        public async Task<CreatedAtRoute<UserDTO>> Create(CreateUserDTO user, CancellationToken cancellationToken)
        {
            // implicitly returns BadRequest() if the Body of the request is not valid

            var createdUser = await _userService.CreateAsync(user, cancellationToken);

            return TypedResults.CreatedAtRoute(
                createdUser,
                nameof(GetById),
                new { id = createdUser.Id }
            );
        }

        [HttpPut("{id}")]
        public async Task<Results<NoContent, NotFound<ProblemDetails>, BadRequest<ProblemDetails>>> Update(int id, UpdateUserDTO user, CancellationToken cancellationToken)
        {
            // implicitly returns BadRequest() if the Body of the request is not valid

            if (id != user.Id)
            {
                return TypedResults.BadRequest(new ProblemDetails
                {
                    Title = "Invalid request",
                    Detail = "The Id in the route does not match the Id in the request body."
                });
            }            

            // Not sure if this check should be here, or in the service
            var existingUser = await _userService.GetByIdAsync(id, cancellationToken);

            if (existingUser is null)
            {
                return TypedResults.NotFound(new ProblemDetails
                {
                    Title = "User not found",
                    Detail = "No user found with the given Id."
                });
            }


            await _userService.UpdateAsync(id, user, cancellationToken);

            return TypedResults.NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<Results<NoContent, NotFound<ProblemDetails>>> Delete(int id, CancellationToken cancellationToken)
        {
            // Not sure if this check should be here, or in the service
            var existingUser = await _userService.GetByIdAsync(id, cancellationToken);

            if (existingUser is null)
            {
                return TypedResults.NotFound(new ProblemDetails
                {
                    Title = "Invalid request",
                    Detail = "The Id in the route does not match the Id in the request body."
                });
            }


            await _userService.DeleteAsync(id, cancellationToken);

            return TypedResults.NoContent();
        }
    }
}
