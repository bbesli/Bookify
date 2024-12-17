using Bookify.Domain.Abstractions;
using SharedKernel;

namespace Bookify.Domain.Bookings.Events;

public record BookingRejectedDomainEvent(Guid bookingId) : IDomainEvent;