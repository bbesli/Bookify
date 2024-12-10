using Bookify.Application.Abstractions.Email;

namespace Infrastructure.Email;

internal sealed class EmailService : IEmailService
{
    public Task SendEmailAsync(Bookify.Domain.Users.Email recipient, string subject, string body)
    {
        return Task.CompletedTask;
    }
}