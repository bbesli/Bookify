using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.Users.LogInUser
{
    public sealed record LogInUserRequest(string Email, string Password);

}
