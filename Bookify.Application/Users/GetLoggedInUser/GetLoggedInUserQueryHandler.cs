using Bookify.Application.Abstractions.Authentication;
using Bookify.Application.Abstractions.Data;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.Users.GetLoggedInUser
{
    internal sealed class GetLoggedInUserQueryHandler
    : IQueryHandler<GetLoggedInUserQuery, UserResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IUserContext _userContext;

        public GetLoggedInUserQueryHandler(
            IApplicationDbContext context,
            IUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public async Task<Result<UserResponse>> Handle(
            GetLoggedInUserQuery request,
            CancellationToken cancellationToken)
        {
            UserResponse user = await _context.Users
                .Where(u => u.IdentityId == _userContext.IdentityId)
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    FirstName = u.FirstName.Value,
                    LastName = u.LastName.Value,
                    Email = u.Email.Value
                })
                .SingleAsync(cancellationToken);

            return user;
        }
    }
}
