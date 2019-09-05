using FluentValidation.TestHelper;
using NoteTaker.API.ViewModelValidators;
using Xunit;

namespace NoteTaker.API.Tests.Unit.ViewModelValidators
{
    public class UpdateNoteValidatorTests
    {
        private readonly UpdateNoteValidator validator;

        public UpdateNoteValidatorTests()
        {
            validator = new UpdateNoteValidator();
        }

        [Fact]
        public void Should_Error_When_Title_Is_Null()
        {
            validator.ShouldHaveValidationErrorFor(x => x.Title, null as string);
        }

        [Fact]
        public void Should_Error_When_Content_Is_Null()
        {
            validator.ShouldHaveValidationErrorFor(x => x.Content, null as string);
        }
    }
}
