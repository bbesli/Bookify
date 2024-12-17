using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.Users.RegisterUser
{
    public sealed record RegisterUserRequest(
    string Email,
    string FirstName,
    string LastName,
    string Password);
}
