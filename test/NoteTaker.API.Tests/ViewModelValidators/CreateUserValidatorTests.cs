using FluentValidation.TestHelper;
using NoteTaker.API.ViewModelValidators;
using Xunit;

namespace NoteTaker.API.Tests.ViewModelValidators
{
    public class CreateUserValidatorTests
    {
        private readonly CreateUserValidator validator;

        public CreateUserValidatorTests()
        {
            this.validator = new CreateUserValidator();
        }

        [Fact]
        public void Should_Error_When_Username_Is_Null()
        {
            validator.ShouldHaveValidationErrorFor(x => x.Username, null as string);
        }

        [Fact]
        public void Should_Error_When_FirstName_Is_Null()
        {
            validator.ShouldHaveValidationErrorFor(x => x.FirstName, null as string);
        }

        [Fact]
        public void Should_Error_When_LastName_Is_Null()
        {
            validator.ShouldHaveValidationErrorFor(x => x.LastName, null as string);
        }

        [Fact]
        public void Should_Error_When_Password_Is_Null()
        {
            validator.ShouldHaveValidationErrorFor(x => x.Password, null as string);
        }

        [Fact]
        public void Should_Error_When_Password_Is_Too_Short()
        {
            validator.ShouldHaveValidationErrorFor(x => x.Password, new string('\t', 5));
        }

        [Fact]
        public void Should_Error_When_Password_Is_Too_Long()
        {
            validator.ShouldHaveValidationErrorFor(x => x.Password, new string('\t', 61));
        }
    }
}
