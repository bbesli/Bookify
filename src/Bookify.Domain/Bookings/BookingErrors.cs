using Bookify.Domain.Abstractions;
using SharedKernel;

namespace Bookify.Domain.Bookings;

public static class BookingErrors
{
    public static Error NotFound(Guid bookingId) => Error.NotFound(
        "Booking.Found",
        $"The booking with the Id = '{bookingId}' was not found."
        );

    public static Error Overlap => Error.Conflict(
        "Booking.Overlap",
        $"The current booking is overlapping with an existing booking."
        );
    public static Error NotReserved => Error.Problem(
        "Booking.NotReserved",
        $"The booking is not reserved."
        );
    public static Error NotConfirmed => Error.Problem(
        "Booking.NotConfirmed",
        $"The booking is not confirmed."
        );
    public static Error AlreadyStarted => Error.Problem(
        "Booking.AlreadyStarted",
        $"The booking has already been started."
        );
}