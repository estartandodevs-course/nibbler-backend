using Nibbler.Core.DomainObjects;

namespace Nibbler.Tarefa.Domain;

public class ItemTarefa : Entity
{
    public Guid TarefaId { get; private set; }
    public Guid UsuarioId { get; private set; }
    public string Descricao { get; private set; }
    public bool Concluido { get; private set; }
    public int Ordem { get; private set; }
    
    // Navegação EF
    public Tarefa Tarefa { get; private set; }

    protected ItemTarefa() { }

    public ItemTarefa(Guid tarefaId, Guid usuarioId, string descricao, int ordem)
    {
        TarefaId = tarefaId;
        UsuarioId = usuarioId;
        Descricao = descricao;
        Ordem = ordem;
        Concluido = false;
    }

    public void MarcarComoConcluido()
    {
        Concluido = true;
        DataDeAlteracao = DateTime.Now;
    }

    public void DesmarcarComoConcluido()
    {
        Concluido = false;
        DataDeAlteracao = DateTime.UtcNow;
    }

    public void AlterarDescricao(string descricao)
    {
        Descricao = descricao;
        DataDeAlteracao = DateTime.UtcNow;
    }

    public void AlterarOrdem(int ordem)
    {
        Ordem = ordem;
        DataDeAlteracao = DateTime.UtcNow;
    }
}