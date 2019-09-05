using FluentValidation;
using NoteTaker.API.ViewModels;

namespace NoteTaker.API.ViewModelValidators
{
    public class NoteValidator : AbstractValidator<Note>
    {
        public NoteValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Title).NotNull();
            RuleFor(x => x.Content).NotNull();
        }
    }
}
