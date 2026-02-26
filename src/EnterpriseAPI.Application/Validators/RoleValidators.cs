using EnterpriseAPI.Application.DTOs;
using FluentValidation;

namespace EnterpriseAPI.Application.Validators;

public class CreateRoleValidator : AbstractValidator<CreateRoleDto>
{
    public CreateRoleValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Rol adı boş olamaz.")
            .MaximumLength(50).WithMessage("Rol adı en fazla 50 karakter olabilir.");
    }
}

public class UpdateRoleValidator : AbstractValidator<UpdateRoleDto>
{
    public UpdateRoleValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Rol adı boş olamaz.")
            .MaximumLength(50).WithMessage("Rol adı en fazla 50 karakter olabilir.");
    }
}