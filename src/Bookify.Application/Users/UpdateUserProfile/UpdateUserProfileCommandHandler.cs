using Bookify.Application.Abstractions.Authentication;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Users.GetLoggedInUser;
using Bookify.Domain.Entities.Abstractions;
using Bookify.Domain.Entities.Users;
using Mapster;

namespace Bookify.Application.Users;

internal sealed class RegisterUserCommandHandler
    : ICommandHandler<UpdateUserProfileCommand, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public RegisterUserCommandHandler(IAuthenticationService authenticationService, IUserRepository userRepository, IUnitOfWork unitOfWork, IUserContext userContext)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<Result<UserResponse>> Handle(UpdateUserProfileCommand command, CancellationToken cancellationToken)
    {
        var user = command.Adapt<User>();
        var userId = new UserId(_userContext.UserId);
        User updatedUser = await _userRepository.Update(userId, user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return updatedUser.Adapt<UserResponse>();
    }
}