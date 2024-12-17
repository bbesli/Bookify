using Bookify.Domain.Abstractions;
using SharedKernel;

namespace Bookify.Domain.Bookings.Events;

public record BookingCancelledDomainEvent(Guid bookingId) : IDomainEvent;