using FluentValidation.TestHelper;
using NoteTaker.API.ViewModelValidators;
using Xunit;

namespace NoteTaker.API.Tests.ViewModelValidators
{
    public class NoteValidatorTests
    {
        private readonly NoteValidator validator;

        public NoteValidatorTests()
        {
            this.validator = new NoteValidator();
        }

        [Fact]
        public void Should_Error_When_Id_Is_Null()
        {
            validator.ShouldHaveValidationErrorFor(x => x.Id, null as string);
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
