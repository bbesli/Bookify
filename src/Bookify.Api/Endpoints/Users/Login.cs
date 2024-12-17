using Bookify.Api.Extensions;
using Bookify.Api.Infrastructure;
using Bookify.Application.Users.LogInUser;
using Bookify.Application.Users.RegisterUser;
using MediatR;
using SharedKernel;

namespace Bookify.Api.Endpoints.Users
{
    public class Login : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("users/login", async (
                LogInUserRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new LogInUserCommand(
                    request.Email,
                    request.Password);

                Result<AccessTokenResponse> result = await sender.Send(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
                .WithTags(Tags.Users);
        }
    }
}
