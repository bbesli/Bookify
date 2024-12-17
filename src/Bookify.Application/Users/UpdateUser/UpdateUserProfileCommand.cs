using Bookify.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.Users.UpdateUser
{
    public sealed record UpdateUserProfileCommand(Guid UserId, string FirstName, string LastName) : ICommand;
}
