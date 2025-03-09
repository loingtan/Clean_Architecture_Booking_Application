using Bookify.Application.Abstractions.Data;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Common.Models;
using Bookify.Domain.Entities.Abstractions;
using Bookify.Domain.Entities.Bookings.Enums;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bookify.Application.Apartments.SearchApartments;

internal sealed class SearchApartmentQueryHandler
    : IQueryHandler<SearchApartmentsQuery, PagedResponse<ApartmentResponse>>
{
    private static readonly int[] ActiveBookingStatuses =
    {
        (int)BookingStatus.Reserved,
        (int)BookingStatus.Confirmed,
        (int)BookingStatus.Completed
    };

    private readonly ISqlConnectionFactory _connectionFactory;

    public SearchApartmentQueryHandler(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Result<PagedResponse<ApartmentResponse>>> Handle(
        SearchApartmentsQuery request,
        CancellationToken cancellationToken)
    {
        if (request.StartDate > request.EndDate)
            return new PagedResponse<ApartmentResponse>(
                new List<ApartmentResponse>(), 0, request.PageSize, request.PageNumber, 0, false, false);

        using var connection = _connectionFactory.CreateConnection();

        var sqlCount = """
            SELECT COUNT(a.id)
            FROM apartments a
            WHERE NOT EXISTS
            (
                SELECT 1
                FROM bookings b
                WHERE
                    b.apartment_id = a.id AND
                    b.duration_start <= @EndDate AND
                    b.duration_end >= @StartDate AND
                    b.status = ANY(@ActiveBookingStatuses)
            )
            """;

        if (!string.IsNullOrEmpty(request.Country))
        {
            sqlCount += " AND a.address_country = @Country";
        }

        if (!string.IsNullOrEmpty(request.City))
        {
            sqlCount += " AND a.address_city = @City";
        }

        var totalCount = await connection.ExecuteScalarAsync<int>(
            sqlCount,
            new
            {
                request.StartDate,
                request.EndDate,
                ActiveBookingStatuses,
                request.Country,
                request.City
            });

        var sql = """
            SELECT
                a.id AS Id,
                a.name AS Name,
                a.description AS Description,
                a.price_amount AS Price,
                a.price_currency AS Currency,
                a.address_country AS Country,
                a.address_state AS State,
                a.address_zip_code AS ZipCode,
                a.address_city AS City,
                a.address_street AS Street
            FROM apartments a
            WHERE NOT EXISTS
            (
                SELECT 1
                FROM bookings b
                WHERE
                    b.apartment_id = a.id AND
                    b.duration_start <= @EndDate AND
                    b.duration_end >= @StartDate AND
                    b.status = ANY(@ActiveBookingStatuses)
            )
            """;

        if (!string.IsNullOrEmpty(request.Country))
        {
            sql += " AND a.address_country = @Country";
        }

        if (!string.IsNullOrEmpty(request.City))
        {
            sql += " AND a.address_city = @City";
        }

        sql += """
            ORDER BY a.id
            LIMIT @PageSize OFFSET GREATEST((@PageNumber - 1) * @PageSize, 0);
            """;

        var apartments = await connection.QueryAsync<ApartmentResponse>(
            sql,
            new
            {
                request.StartDate,
                request.EndDate,
                ActiveBookingStatuses,
                request.Country,
                request.City,
                request.PageSize,
                request.PageNumber
            });

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);
        var hasNext = request.PageNumber < totalPages;
        var hasPrevious = request.PageNumber > 1;

        var pagedResponse = new PagedResponse<ApartmentResponse>(
            apartments.ToList(),
            totalCount,
            request.PageSize,
            request.PageNumber,
            totalPages,
            hasNext,
            hasPrevious);

        return Result.Success(pagedResponse);
    }
}