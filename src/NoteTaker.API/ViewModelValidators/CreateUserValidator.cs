using FluentValidation;
using NoteTaker.API.ViewModels;

namespace NoteTaker.API.ViewModelValidators
{
    public class CreateUserValidator : AbstractValidator<CreateUser>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Password).MinimumLength(6).MaximumLength(60).NotNull();
        }
    }
}
