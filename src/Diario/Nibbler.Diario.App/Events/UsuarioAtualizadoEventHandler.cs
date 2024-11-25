using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nibbler.Core.Messages;
using Nibbler.Diario.Domain.Interfaces;

namespace Nibbler.Diario.app.Events;

public class UsuarioAtualizadoEventHandler : INotificationHandler<UsuarioAtualizadoEvent>
{
    private readonly IServiceProvider _serviceProvider;

    public UsuarioAtualizadoEventHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task Handle(UsuarioAtualizadoEvent notification, CancellationToken cancellationToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var postagemRepository = scope.ServiceProvider.GetRequiredService<IDiarioRepository>();

            var usuario = new Domain.Usuario(notification.UsuarioId, notification.Nome, notification.Foto);
            
            postagemRepository.Atualizar(usuario);

            await Task.CompletedTask;
        }
    }
}
