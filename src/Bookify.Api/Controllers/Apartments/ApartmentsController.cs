using Asp.Versioning;
using Bookify.Application.Apartments.SearchAparments;
using Bookify.Domain.Apartments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers.Apartments;

public static class ApartmentsController
{

    public static IEndpointRouteBuilder MapApartmentEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("apartments", SearchApartments)
            .RequireAuthorization("apartments:read")
            .WithName(nameof(SearchApartments));

        return builder;
    }

    public static async Task<IResult> SearchApartments(
        DateOnly startDate, 
        DateOnly endDate,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new SearchApartmentsQuery(startDate, endDate);
        
        var result = await sender.Send(query, cancellationToken);
        
        return Results.Ok(result.Value);
    }
}