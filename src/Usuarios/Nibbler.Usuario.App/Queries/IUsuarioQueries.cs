using Nibbler.Tarefa.App.ViewModels;
using Nibbler.Usuario.Domain.Interfaces;

namespace Nibbler.Usuario.App.Queries;

public interface IUsuarioQueries
{
    Task<IEnumerable<UsuarioViewModel>> ObterTodos();

    Task<UsuarioViewModel> ObterPorId(Guid Id);
}

public class UsuarioQueries : IUsuarioQueries
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioQueries(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<UsuarioViewModel> ObterPorId(Guid Id)
    {
        var usuario = await _usuarioRepository.ObterPorId(Id);

        return UsuarioViewModel.Mapear(usuario);
    }

    public async Task<IEnumerable<UsuarioViewModel>> ObterTodos()
    {
        var usuarios = await _usuarioRepository.ObterTodos();

        return usuarios.Select(UsuarioViewModel.Mapear);
    }
}
