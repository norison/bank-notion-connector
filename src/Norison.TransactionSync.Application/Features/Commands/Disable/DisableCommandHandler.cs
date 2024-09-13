using MediatR;

using Monobank.Client;

using Norison.TransactionSync.Application.Services.Users;

namespace Norison.TransactionSync.Application.Features.Commands.Disable;

public class DisableCommandHandler(IUsersService usersService, IMonobankClient monobankClient)
    : IRequestHandler<DisableCommand>
{
    public async Task Handle(DisableCommand request, CancellationToken cancellationToken)
    {
        var user = await usersService.GetUserByChatIdAsync(request.ChatId, cancellationToken);

        if (user is null)
        {
            throw new InvalidOperationException("User not found.");
        }

        await monobankClient.Personal.SetWebHookAsync("", user.MonoToken, cancellationToken);
    }
}