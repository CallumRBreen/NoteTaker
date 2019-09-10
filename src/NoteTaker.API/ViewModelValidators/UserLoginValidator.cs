using FluentValidation;
using NoteTaker.API.ViewModels;

namespace NoteTaker.API.ViewModelValidators
{
    public class UserLoginValidator : AbstractValidator<UserLogin>
    {
        public UserLoginValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
