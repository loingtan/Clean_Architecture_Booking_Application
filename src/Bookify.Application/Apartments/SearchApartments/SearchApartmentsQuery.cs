using System.Collections.Generic;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Common.Models;

namespace Bookify.Application.Apartments.SearchApartments;

public record SearchApartmentsQuery(
    DateOnly StartDate,
    DateOnly EndDate,
    int PageNumber = 1,
    int PageSize = 10,
    string? Country = null,
    string? City = null)
    : IQuery<PagedResponse<ApartmentResponse>>;

public record PagedResponse<T>(
    IReadOnlyCollection<T> Items,
    int TotalCount,
    int PageSize,
    int CurrentPage,
    int TotalPages,
    bool HasNext,
    bool HasPrevious);