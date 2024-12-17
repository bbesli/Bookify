using Bookify.Api.Extensions;
using Bookify.Api.Infrastructure;
using Bookify.Application.Users.UpdateUser;
using MediatR;
using SharedKernel;

namespace Bookify.Api.Endpoints.Users
{
    public class Update : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("users/updateProfile", async (
                UpdateUserProfileRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateUserProfileCommand(
                request.UserId,
                request.FirstName,
                request.LastName);

                Result result = await sender.Send(command, cancellationToken);

                return result.Match(onSuccess: () => Results.Ok(), CustomResults.Problem);
            })
                .RequireAuthorization(Permissions.UsersRead)
                .WithTags(Tags.Users);
        }
    }
}
