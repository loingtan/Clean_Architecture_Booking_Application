using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Exceptions;
using Bookify.Domain.Apartments;
using Bookify.Domain.Entities.Abstractions;
using Bookify.Domain.Entities.Apartments;
using Bookify.Domain.Entities.Bookings;
using Bookify.Domain.Entities.Bookings.ValueObjects;
using Bookify.Domain.Entities.Users;

namespace Bookify.Application.Bookings.ReserveBooking;

public sealed class ReserveBookingCommandHandler(
    IUserRepository userRepository,
    IApartmentRepository apartmentRepository,
    IBookingRepository bookingRepository,
    IUnitOfWork unitOfWork,
    PricingService pricingService,
    IDateTimeProvider dateProvider)
    : ICommandHandler<ReserveBookingCommand, Guid>
{
    public async Task<Result<Guid>> Handle(ReserveBookingCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(new UserId(request.UserId), cancellationToken);
        if (user is null)
            return Result.Failure<Guid>(UserErrors.NotFound);

        var apartment = await apartmentRepository.GetByIdAsync(new ApartmentId(request.ApartmentId), cancellationToken);
        if (apartment is null)
            return Result.Failure<Guid>(ApartmentErrors.NotFound);

        var duration = DateRange.From(request.StartDate, request.EndDate);

        if (await bookingRepository.IsOverlappingAsync(apartment, duration, cancellationToken))
            return Result.Failure<Guid>(BookingErrors.Overlap);

        try
        {
            var booking = Booking.Reserve(
            apartment,
            user.Id,
            duration,
            dateProvider.UtcNow,
            pricingService);

            bookingRepository.Add(booking);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return booking.Id.Value;
        }
        catch (ConcurrencyException)
        {
            return Result.Failure<Guid>(BookingErrors.Overlap);
        }        
    }
}
