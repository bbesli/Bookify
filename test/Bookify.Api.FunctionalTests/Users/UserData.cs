using Bookify.Api.Controllers.Users;
using Bookify.Application.Users.RegisterUser;

namespace Bookify.Api.FunctionalTests.Users;

internal static class UserData
{
    public static RegisterUserRequest RegisterTestUserRequest = new("test@test.com", "test", "test", "12345");
}
