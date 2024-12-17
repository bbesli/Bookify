using Bookify.Api.Endpoints.Users;
using Bookify.Api.Extensions;
using Bookify.Api.Infrastructure;
using Bookify.Application.Bookings.GetBooking;
using MediatR;

namespace Bookify.Api.Endpoints.Bookings
{
    public class Get : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("bookings/{id}", async (
                Guid id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetBookingQuery(id);

                var result = await sender.Send(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
                .RequireAuthorization(Permissions.UsersRead)
                .WithTags(Tags.Bookings);
        }
    }
}
