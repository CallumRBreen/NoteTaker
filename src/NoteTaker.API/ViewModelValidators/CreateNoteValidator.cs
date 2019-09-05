using FluentValidation;
using NoteTaker.API.ViewModels;

namespace NoteTaker.API.ViewModelValidators
{
    public class CreateNoteValidator : AbstractValidator<CreateNote>
    {
        public CreateNoteValidator()
        {
            RuleFor(x => x.Title).NotNull();
            RuleFor(x => x.Content).NotNull();
        }
    }
}
