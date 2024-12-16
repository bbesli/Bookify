using Asp.Versioning;
using Bookify.Application.Users.GetLoggedInUser;
using Bookify.Application.Users.LogInUser;
using Bookify.Application.Users.RegisterUser;
using Bookify.Application.Users.UpdateUser;
using Bookify.Domain.Abstractions;
using Bookify.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers.Users;

public static class UsersController
{

    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("users/me", GetLoggedInUser)
            .RequireAuthorization("users:read")
            .WithName(nameof(GetLoggedInUser));

        builder.MapPost("users/register", Register)
            .AllowAnonymous()
            .WithName(nameof(Register));

        builder.MapPost("users/login", LogIn)
            .AllowAnonymous()
            .WithName(nameof(LogIn));

        builder.MapPut("users/profile", UpdateProfile)
            .RequireAuthorization("users:read")
            .WithName(nameof(UpdateProfile));

        return builder;
    }

    public static async Task<IResult> GetLoggedInUser(ISender sender, CancellationToken cancellationToken)
    {
        var query = new GetLoggedInUserQuery();

        var result = await sender.Send(query, cancellationToken);

        return Results.Ok(result.Value);
    }

    public static async Task<IResult> Register(
        RegisterUserRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(
            request.Email,
            request.FirstName,
            request.LastName,
            request.Password);

        Result<Guid> result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return Results.BadRequest(result.Error);
        }

        return Results.Ok(result.Value);
    }

    public static async Task<IResult> LogIn(
        LogInUserRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new LogInUserCommand(request.Email, request.Password);
    
        Result<AccessTokenResponse> result = await sender.Send(command, cancellationToken);
    
        if (result.IsFailure)
        {
            return Results.Unauthorized();
        }
    
        return Results.Ok(result.Value);
    }

    public static async Task<IResult> UpdateProfile(
        UpdateUserProfileRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new UpdateUserProfileCommand(
                request.UserId,
                request.FirstName,
                request.LastName);

        Result result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return Results.BadRequest(result.Error);
        }

        return Results.Ok();
    }
}