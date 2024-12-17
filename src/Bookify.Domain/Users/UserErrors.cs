using Bookify.Domain.Abstractions;
using SharedKernel;

namespace Bookify.Domain.Users;

public static class UserErrors
{
    public static Error NotFound(Guid userId) => Error.NotFound(
        "User.Found",
        $"The user with the ID = '{userId}' was not found.");

    public static Error InvalidCredentials => Error.Problem(
        "User.InvalidCredentials",
        "The provided credentials were invalid.");
}