using FluentValidation.TestHelper;
using NoteTaker.API.ViewModelValidators;
using Xunit;

namespace NoteTaker.API.Tests.ViewModelValidators
{
    public class UserLoginValidatorTests
    {
        private readonly UserLoginValidator validator;

        public UserLoginValidatorTests()
        {
            validator = new UserLoginValidator();
        }

        [Fact]
        public void Should_Error_When_Username_Is_Null()
        {
            validator.ShouldHaveValidationErrorFor(x => x.Username, null as string);
        }

        [Fact]
        public void Should_Error_When_Password_Is_Null()
        {
            validator.ShouldHaveValidationErrorFor(x => x.Password, null as string);
        }

    }
}
