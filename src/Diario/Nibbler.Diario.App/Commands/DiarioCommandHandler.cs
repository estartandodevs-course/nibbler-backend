using FluentValidation.Results;
using MediatR;
using Nibbler.Core.Messages;
using Nibbler.Diario.app.Commands;
using Nibbler.Diario.app.Events;
using Nibbler.Diario.Domain;
using Nibbler.Diario.Domain.Events;
using Nibbler.Diario.Domain.Interfaces;
using Nibbler.Usuario.Domain.Interfaces;

namespace Nibbler.Diario.App.Commands;

public class DiarioCommandHandler : CommandHandler,
    IRequestHandler<AdicionarDiarioCommand, ValidationResult>,
    IRequestHandler<AtualizarDiarioCommand, ValidationResult>,
    IRequestHandler<AdicionarReflexaoCommand, ValidationResult>,
    IRequestHandler<AtualizarReflexaoCommand, ValidationResult>,
    IRequestHandler<ExcluirReflexaoCommand, ValidationResult>,
    IRequestHandler<MarcarComoExcluidoCommand, ValidationResult>,
    IRequestHandler<AdicionarEntradaCommand, ValidationResult>,
    IRequestHandler<AtualizarEntradaCommand, ValidationResult>,
    IRequestHandler<RemoverEntradaCommand, ValidationResult>,
    IRequestHandler<AdicionarEtiquetaCommand, ValidationResult>,
    IRequestHandler<CriarEtiquetaCommand, ValidationResult>,
    IRequestHandler<AtualizarEtiquetaCommand, ValidationResult>,
    IRequestHandler<RemoverEtiquetaDoDiarioCommand, ValidationResult>,
    IRequestHandler<AdicionarEtiquetaAoDiarioCommand, ValidationResult>,
    IRequestHandler<ExcluirEtiquetaCommand, ValidationResult>,
    IRequestHandler<AdicionarEmocaoCommand, ValidationResult>,
    IRequestHandler<AtualizarEmocaoCommand, ValidationResult>,
    IRequestHandler<ExcluirEmocaoCommand, ValidationResult>,
    IRequestHandler<AssociarEmocaoNaReflexaoCommand, ValidationResult>,
    IRequestHandler<RemoverEmocaoDaReflexaoCommand, ValidationResult>,
    IDisposable
{
    private readonly IDiarioRepository _diarioRepository;
    private readonly IUsuarioRepository _usuarioRepository;

    public DiarioCommandHandler(IDiarioRepository diarioRepository, IUsuarioRepository usuarioRepository)
    {
        _diarioRepository = diarioRepository;
        _usuarioRepository = usuarioRepository;
    }

    public async Task<ValidationResult> Handle(AdicionarDiarioCommand request, CancellationToken cancellationToken)
    {
        if (!request.EstaValido()) return request.ValidationResult;

        if (await DiarioExiste(request.Titulo, request.UsuarioId))
        {
            AdicionarErro(
                "Já existe um diário com este título para este usuário. Por favor, escolha um título diferente.");
            return ValidationResult;
        }

        var usuario = await _diarioRepository.ObterUsuarioDiarioPorId(request.UsuarioId);
        if (usuario == null)
        {
            var usuarioOriginal = await _usuarioRepository.ObterPorId(request.UsuarioId);
            if (usuarioOriginal == null)
            {
                AdicionarErro("Usuário não encontrado!");
                return ValidationResult;
            }

            usuario = new Domain.Usuario(usuarioOriginal.Id, usuarioOriginal.Nome, usuarioOriginal.Foto);
        }

        var diario = new Domain.Diario(usuario, request.Titulo, request.Conteudo);

        _diarioRepository.Adicionar(diario);
        diario.AdicionarEvento(new DiarioCriadoEvent(diario.Id, usuario.Id));

        return await PersistirDados(_diarioRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(AtualizarDiarioCommand request, CancellationToken cancellationToken)
    {
        if (!request.EstaValido()) return request.ValidationResult;

        var diario = await _diarioRepository.ObterPorId(request.DiarioId);

        if (diario is null)
        {
            AdicionarErro("Diário não encontrado!");
            return ValidationResult;
        }

        var diarioAtualizado = new Domain.Diario(diario.Usuario, request.Titulo, request.Conteudo);
        diarioAtualizado.Id = diario.Id;

        _diarioRepository.Atualizar(diarioAtualizado);

        return await PersistirDados(_diarioRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(AdicionarReflexaoCommand request, CancellationToken cancellationToken)
    {
        if (!request.EstaValido()) return request.ValidationResult;

        var reflexao = new Reflexao(request.UsuarioId, request.Conteudo, request.EmocaoId);
        
        _diarioRepository.Adicionar(reflexao);

        return await PersistirDados(_diarioRepository.UnitOfWork);
    }
    
    public async Task<ValidationResult> Handle(AtualizarReflexaoCommand request, CancellationToken cancellationToken)
    {
        if (!request.EstaValido()) return request.ValidationResult;

        var reflexao = await _diarioRepository.ObterReflexaoPorId(request.Id);
        if (reflexao == null)
        {
            AdicionarErro("Reflexão não encontrada!");
            return ValidationResult;
        }

        reflexao.AtualizarConteudo(request.Conteudo);
        reflexao.EmocaoId = request.EmocaoId;

        _diarioRepository.Atualizar(reflexao);

        return await PersistirDados(_diarioRepository.UnitOfWork);
    }
    
    public async Task<ValidationResult> Handle(ExcluirReflexaoCommand request, CancellationToken cancellationToken)
    {
        if (!request.EstaValido()) return request.ValidationResult;

        var reflexao = await _diarioRepository.ObterReflexaoPorId(request.Id);
        if (reflexao == null)
        {
            AdicionarErro("Reflexão não encontrada!");
            return ValidationResult;
        }

        _diarioRepository.Excluir(reflexao);

        return await PersistirDados(_diarioRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(AdicionarEntradaCommand request, CancellationToken cancellationToken)
    {
        if (!request.EstaValido()) return request.ValidationResult;

        var diario = await _diarioRepository.ObterPorId(request.DiarioId);

        if (diario is null)
        {
            AdicionarErro("Diário não encontrado!");
            return ValidationResult;
        }

        var entrada = new Domain.Entrada(request.DiarioId, request.Conteudo);
        diario.AdicionarEntrada(entrada);

        _diarioRepository.Atualizar(diario);

        return await PersistirDados(_diarioRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(AtualizarEntradaCommand request, CancellationToken cancellationToken)
    {
        if (!request.EstaValido()) return request.ValidationResult;

        var diario = await _diarioRepository.ObterPorId(request.DiarioId);

        if (diario is null)
        {
            AdicionarErro("Diário não encontrado!");
            return ValidationResult;
        }

        var entrada = diario.Entradas.FirstOrDefault(e => e.Id == request.EntradaId);

        if (entrada is null)
        {
            AdicionarErro("Entrada não encontrada!");
            return ValidationResult;
        }

        entrada.AtualizarConteudo(request.Conteudo);
        _diarioRepository.Atualizar(diario);

        return await PersistirDados(_diarioRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(RemoverEntradaCommand request, CancellationToken cancellationToken)
    {
        if (!request.EstaValido()) return request.ValidationResult;

        var diario = await _diarioRepository.ObterPorId(request.DiarioId);

        if (diario is null)
        {
            AdicionarErro("Diário não encontrado!");
            return ValidationResult;
        }

        var entrada = diario.Entradas.FirstOrDefault(e => e.Id == request.EntradaId);

        if (entrada is null)
        {
            AdicionarErro("Entrada não encontrada!");
            return ValidationResult;
        }

        diario.RemoverEntrada(entrada);
        _diarioRepository.Atualizar(diario);

        return await PersistirDados(_diarioRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(MarcarComoExcluidoCommand request, CancellationToken cancellationToken)
    {
        if (!request.EstaValido()) return request.ValidationResult;

        var diario = await _diarioRepository.ObterPorId(request.DiarioId);

        if (diario is null)
        {
            AdicionarErro("Diário não encontrado!");
            return ValidationResult;
        }

        if (diario.FoiExcluido())
        {
            AdicionarErro("Este diário já está marcado como excluído!");
            return ValidationResult;
        }

        await _diarioRepository.MarcarComoExcluidoAsync(request.DiarioId);

        return await PersistirDados(_diarioRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(AdicionarEtiquetaCommand request, CancellationToken cancellationToken)
    {
        if (!request.EstaValido()) return request.ValidationResult;

        var diario = await _diarioRepository.ObterPorId(request.DiarioId);

        if (diario is null)
        {
            AdicionarErro("Diário não encontrado!");
            return ValidationResult;
        }

        var etiqueta = new Etiqueta(request.Nome);
        diario.AdicionarEtiqueta(etiqueta);

        _diarioRepository.Atualizar(diario);

        return await PersistirDados(_diarioRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(CriarEtiquetaCommand request, CancellationToken cancellationToken)
    {
        if (!request.EstaValido()) return request.ValidationResult;

        var etiqueta = new Etiqueta(request.Nome);
        _diarioRepository.Adicionar(etiqueta);

        return await PersistirDados(_diarioRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(RemoverEtiquetaDoDiarioCommand request,
        CancellationToken cancellationToken)
    {
        if (!request.EstaValido()) return request.ValidationResult;

        var diario = await _diarioRepository.ObterPorId(request.DiarioId);
        if (diario is null)
        {
            AdicionarErro("Diário não encontrado!");
            return ValidationResult;
        }

        var etiqueta = diario.Etiquetas.FirstOrDefault(e => e.Id == request.EtiquetaId);
        if (etiqueta is null)
        {
            AdicionarErro("Etiqueta não encontrada no diário!");
            return ValidationResult;
        }

        diario.Remover(etiqueta);
        _diarioRepository.Atualizar(diario);

        return await PersistirDados(_diarioRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(AdicionarEtiquetaAoDiarioCommand request,
        CancellationToken cancellationToken)
    {
        if (!request.EstaValido()) return request.ValidationResult;

        var diario = await _diarioRepository.ObterPorId(request.DiarioId);
        if (diario is null)
        {
            AdicionarErro("Diário não encontrado!");
            return ValidationResult;
        }

        var etiqueta = await _diarioRepository.ObterEtiquetaPorId(request.EtiquetaId);
        if (etiqueta is null)
        {
            AdicionarErro("Etiqueta não encontrada!");
            return ValidationResult;
        }

        diario.AdicionarEtiqueta(etiqueta);
        _diarioRepository.Atualizar(diario);

        return await PersistirDados(_diarioRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ExcluirEtiquetaCommand request, CancellationToken cancellationToken)
    {
        if (!request.EstaValido()) return request.ValidationResult;

        var etiqueta = await _diarioRepository.ObterEtiquetaPorId(request.Id);
        if (etiqueta == null)
        {
            AdicionarErro("Etiqueta não encontrada!");
            return ValidationResult;
        }

        // Usando o repositório ao invés do contexto diretamente
        await _diarioRepository.ExcluirEtiqueta(request.Id);
    
        return await PersistirDados(_diarioRepository.UnitOfWork);
    }

    private async Task<bool> DiarioExiste(string titulo, Guid usuarioId)
    {
        var diarios = await _diarioRepository.ObterPorUsuario(usuarioId);
        return diarios.Any(d => d.Titulo.Equals(titulo, StringComparison.InvariantCultureIgnoreCase));
    }

    public async Task<ValidationResult> Handle(AtualizarEtiquetaCommand request, CancellationToken cancellationToken)
    {
        if (!request.EstaValido()) return request.ValidationResult;

        var etiqueta = await _diarioRepository.ObterEtiquetaPorId(request.EtiquetaId);
        if (etiqueta is null)
        {
            AdicionarErro("Etiqueta não encontrada!");
            return ValidationResult;
        }

        etiqueta.AtualizarNome(request.Nome);
        _diarioRepository.Atualizar(etiqueta);

        return await PersistirDados(_diarioRepository.UnitOfWork);
    }
        public async Task<ValidationResult> Handle(AdicionarEmocaoCommand request, CancellationToken cancellationToken)
    {
        if (!request.EstaValido()) return request.ValidationResult;

        var emocao = new Emocao(request.Nome);
        _diarioRepository.Adicionar(emocao);

        return await PersistirDados(_diarioRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(AtualizarEmocaoCommand request, CancellationToken cancellationToken)
    {
        if (!request.EstaValido()) return request.ValidationResult;

        var emocao = await _diarioRepository.ObterEmocaoPorId(request.Id);
        if (emocao == null)
        {
            AdicionarErro("Emoção não encontrada!");
            return ValidationResult;
        }

        emocao.AtualizarNome(request.Nome);
        _diarioRepository.Atualizar(emocao);

        return await PersistirDados(_diarioRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ExcluirEmocaoCommand request, CancellationToken cancellationToken)
    {
        if (!request.EstaValido()) return request.ValidationResult;

        var emocao = await _diarioRepository.ObterEmocaoPorId(request.Id);
        if (emocao == null)
        {
            AdicionarErro("Emoção não encontrada!");
            return ValidationResult;
        }

        _diarioRepository.Excluir(emocao);

        return await PersistirDados(_diarioRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(AssociarEmocaoNaReflexaoCommand request, CancellationToken cancellationToken)
    {
        if (!request.EstaValido()) return request.ValidationResult;

        var reflexao = await _diarioRepository.ObterReflexaoPorId(request.ReflexaoId);
        if (reflexao == null)
        {
            AdicionarErro("Reflexão não encontrada!");
            return ValidationResult;
        }

        var emocao = await _diarioRepository.ObterEmocaoPorId(request.EmocaoId);
        if (emocao == null)
        {
            AdicionarErro("Emoção não encontrada!");
            return ValidationResult;
        }

        reflexao.EmocaoId = request.EmocaoId;
        _diarioRepository.Atualizar(reflexao);

        return await PersistirDados(_diarioRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(RemoverEmocaoDaReflexaoCommand request, CancellationToken cancellationToken)
    {
        if (!request.EstaValido()) return request.ValidationResult;

        var reflexao = await _diarioRepository.ObterReflexaoPorId(request.ReflexaoId);
        if (reflexao == null)
        {
            AdicionarErro("Reflexão não encontrada!");
            return ValidationResult;
        }

        reflexao.EmocaoId = null;
        _diarioRepository.Atualizar(reflexao);

        return await PersistirDados(_diarioRepository.UnitOfWork);
    }


    public void Dispose()
    {
        _diarioRepository?.Dispose();
    }
}