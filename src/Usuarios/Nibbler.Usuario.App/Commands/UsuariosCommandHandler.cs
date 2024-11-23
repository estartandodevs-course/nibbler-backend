using FluentValidation.Results;
using MediatR;
using Nibbler.Core.Messages;
using Nibbler.Core.ValueObjects;
using Nibbler.Usuario.Domain;
using Nibbler.Usuario.Domain.Interfaces;

namespace Nibbler.Usuario.App.Commands;


public class UsuariosCommandHandler : CommandHandler,
    IRequestHandler <AdicionarUsuarioCommand, ValidationResult>, IDisposable
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuariosCommandHandler(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<ValidationResult> Handle(AdicionarUsuarioCommand request, CancellationToken cancellationToken)
    {
        if (!request.EstaValido()) return request.ValidationResult;

        var novo = new Usuario.Domain.Usuario(request.Nome, request.Foto, request.DataDeNascimento);
        var login = new Login(new Email(request.Email), new Senha(request.Senha));
        
        novo.AtribuirLogin(login);
        _usuarioRepository.Adicionar(novo);

        return await PersistirDados(_usuarioRepository.UnitOfWork);
    }
    
    public void Dispose()
    {
        _usuarioRepository.Dispose();
    }
}