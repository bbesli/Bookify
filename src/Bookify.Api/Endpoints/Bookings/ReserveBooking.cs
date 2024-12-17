using Bookify.Api.Endpoints.Users;
using Bookify.Api.Extensions;
using Bookify.Api.Infrastructure;
using Bookify.Application.Bookings.ReserveBooking;
using Bookify.Application.Users.RegisterUser;
using MediatR;
using SharedKernel;

namespace Bookify.Api.Endpoints.Bookings
{
    public class ReserveBooking : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("bookings", async (
                ReserveBookingRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new ReserveBookingCommand(
                    request.ApartmentId,
                    request.UserId,
                    request.StartDate,
                    request.EndDate);

                var result = await sender.Send(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
                .RequireAuthorization(Permissions.UsersRead)
                .WithTags(Tags.Bookings);
        }
    }
}
