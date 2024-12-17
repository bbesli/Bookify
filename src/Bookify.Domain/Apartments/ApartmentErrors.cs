using Bookify.Domain.Abstractions;
using SharedKernel;

namespace Bookify.Domain.Apartments;
public static class ApartmentErrors
{
    public static Error NotFound = new(
        "Apartment.Found",
        "The apartment with the specified ID was not found.",
        ErrorType.NotFound);
}