using Bookify.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Infrastructure.Authorization
{
    internal sealed class UserRolesResponse
    {
        public Guid UserId { get; init; }

        public List<Role> Roles { get; init; } = [];
    }
}
