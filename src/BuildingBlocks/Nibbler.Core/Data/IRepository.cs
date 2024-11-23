using Nibbler.Core.DomainObjects;

namespace Nibbler.Core.Data;

public interface IRepository<TEntity> : IDisposable where TEntity : IAggregateRoot
{
    IUnitOfWorks UnitOfWork { get; }

    Task<TEntity> ObterPorId(Guid id);

    void Adicionar(TEntity entity);
  
    void Atualizar(TEntity entity);

    void Apagar(Func<TEntity,bool> predicate);
}
