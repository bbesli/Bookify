using Bookify.Domain.Abstractions;
using SharedKernel;

namespace Bookify.Domain.Reviews;

public static class ReviewErrors
{
    public static Error NotEligible => Error.Problem(
        "Review.NotEligible",
        "The review is not eligible because the booking is not yet completed."
        );

    public static Error NotFound(Review review) => Error.NotFound(
        "Review.NotFound",
        $"The review with the Id = '{review.Id}' was not found.");
}