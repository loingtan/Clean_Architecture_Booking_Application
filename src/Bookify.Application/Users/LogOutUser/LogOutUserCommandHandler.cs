using Bookify.Application.Abstractions.Authentication;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Entities.Abstractions;
using MediatR;

namespace Bookify.Application.Users.LogOutUser;

public class LogOutUserCommandHandler : ICommandHandler<LogOutUserCommand>
{
    private readonly IJwtService _jwtService;
 

    public LogOutUserCommandHandler(IJwtService jwtService)
    {
        _jwtService = jwtService;
      
    }
    public async Task<Result> Handle(LogOutUserCommand request, CancellationToken cancellationToken)
    {
        await _jwtService.InvalidateTokenAsync(cancellationToken);
        return Result.Success();
    }


}