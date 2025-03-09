using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Apartments;
using Bookify.Domain.Entities.Abstractions;
using Bookify.Domain.Entities.Apartments;
using MapsterMapper;

namespace Bookify.Application.Apartments.UpdateApartment;

internal sealed class UpdateApartmentCommandHandler : ICommandHandler<UpdateApartmentCommand>
{
    private readonly IApartmentRepository _apartmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateApartmentCommandHandler(
        IApartmentRepository apartmentRepository,
        IUnitOfWork unitOfWork)
    {
        _apartmentRepository = apartmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateApartmentCommand command, CancellationToken cancellationToken)
    {
        var apartment = await _apartmentRepository.GetByIdAsync(new ApartmentId(command.Id), cancellationToken);

        if (apartment is null)
        {
            return Result.Failure(ApartmentErrors.NotFound);
        }

        apartment.Update(
            name: command.Name,
            description: command.Description,
            priceAmount: command.Price,
            cleaningFeeAmount: command.CleaningFee,
            amenities: null,
            street: command.Address,
            city: command.City,
            state: command.State,
            zipCode: command.ZipCode,
            country: command.Country
        );

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}