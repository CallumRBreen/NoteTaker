using FluentValidation;
using NoteTaker.API.ViewModels;

namespace NoteTaker.API.ViewModelValidators
{
    public class UpdateNoteValidator : AbstractValidator<UpdateNote>
    {
        public UpdateNoteValidator()
        {
            RuleFor(x => x.Title).NotNull();
            RuleFor(x => x.Content).NotNull();
        }
    }
}
