
using Bookify.Api.Extensions;
using Bookify.Api.Infrastructure;
using Bookify.Application.Users.GetLoggedInUser;
using MediatR;

namespace Bookify.Api.Endpoints.Users
{
    public class Me : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("users/me", async (
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetLoggedInUserQuery();

                var result = await sender.Send(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
                .RequireAuthorization(Permissions.UsersRead)
                .WithTags(Tags.Users);
        }
    }
}
