using FluentValidation.Results;
using Nibbler.Core.Messages;

namespace Nibbler.Diario.app.Commands;

public class ExcluirEtiquetaCommand : Command
{
    public Guid Id { get; private set; }

    public ExcluirEtiquetaCommand(Guid id)
    {
        Id = id;
    }

    public override bool EstaValido()
    {
        ValidationResult = new ValidationResult();
        
        if (Id == Guid.Empty)
            ValidationResult.Errors.Add(new ValidationFailure("Id", "ID da etiqueta inválido"));

        return ValidationResult.IsValid;
    }
}