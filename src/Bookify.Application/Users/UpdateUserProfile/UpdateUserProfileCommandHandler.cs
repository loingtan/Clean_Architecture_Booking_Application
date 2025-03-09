using System.Net;
using Bookify.Application.Abstractions.Authentication;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Entities.Abstractions;
using Bookify.Domain.Entities.Users;

namespace Bookify.Application.Users;

internal sealed class RegisterUserCommandHandler
    : ICommandHandler<UpdateUserProfileCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public RegisterUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IUserContext userContext)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<Result> Handle(UpdateUserProfileCommand command, CancellationToken cancellationToken)
    {
        var userId = new UserId(_userContext.UserId);
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound);
        }
        user.Update(command.FirstName, command.LastName);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}