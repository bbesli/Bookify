using Bookify.Domain.Abstractions;
using SharedKernel;

namespace Bookify.Domain.Bookings.Events;

public record BookingConfirmedDomainEvent(Guid bookingId) : IDomainEvent;