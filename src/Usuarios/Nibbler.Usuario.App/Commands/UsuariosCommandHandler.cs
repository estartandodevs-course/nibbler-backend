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
    public async Task<ValidationResult> Handle(AtualizarUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.ObterPorId(request.UsuarioId);
        if (usuario is null)
        {
            AdicionarErro("Usuário não encontrado!");
            return ValidationResult;
        }

        usuario.AtribuirNome(request.Nome);
        usuario.AtribuirFoto(request.Foto);
        usuario.AtribuirDataDeNascimento(request.DataDeNascimento);

        _usuarioRepository.Atualizar(usuario);

        var evento = new UsuarioAtualizadoEvent(usuario.Id,usuario.Nome,usuario.Foto);

        usuario.AdicionarEvento(evento);
    
        return await PersistirDados(_usuarioRepository.UnitOfWork);
    }
    
    public void Dispose()
    {
        _usuarioRepository.Dispose();
    }
}