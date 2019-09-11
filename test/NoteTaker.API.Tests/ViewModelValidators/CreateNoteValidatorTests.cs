using FluentValidation.TestHelper;
using NoteTaker.API.ViewModelValidators;
using Xunit;

namespace NoteTaker.API.Tests.ViewModelValidators
{
    public class CreateNoteValidatorTests
    {
        private readonly CreateNoteValidator validator;

        public CreateNoteValidatorTests()
        {
            validator = new CreateNoteValidator();
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
