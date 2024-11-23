namespace Nibbler.Core.Data;

public interface IUnitOfWorks
{
    Task<bool> Commit();
}