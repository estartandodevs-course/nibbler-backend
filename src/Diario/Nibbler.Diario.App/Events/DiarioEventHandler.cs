using MediatR;
using Microsoft.Extensions.Logging;
using Nibbler.Diario.app.Events;
using Nibbler.Diario.Domain.Events;

namespace Nibbler.Diario.App.Events;

public class DiarioEventHandler : 
    INotificationHandler<DiarioCriadoEvent>
{
    private readonly ILogger<DiarioEventHandler> _logger;

    public DiarioEventHandler(ILogger<DiarioEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(DiarioCriadoEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Novo diário criado - ID: {notification.DiarioId} por Usuário: {notification.UsuarioId} em {notification.DataCriacao}");
        return Task.CompletedTask;
    }
}