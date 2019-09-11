using System;
using FluentAssertions;
using NoteTaker.Core.Services.Implementations;
using Xunit;

namespace NoteTaker.Core.Tests.Services
{
    public class BCryptPasswordHashingServiceTests
    {
        private readonly BCryptPasswordHashingService service;

        public BCryptPasswordHashingServiceTests()
        {
            service = new BCryptPasswordHashingService();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void If_Password_IsNullOrEmpty_ThrowArgumentException(string password)
        {
            Action action = () => service.GetPasswordHash(password);

            action.Should().Throw<ArgumentException>().And.ParamName.Should().Be("password");
        }

        [Theory]
        [InlineData(null,"")]
        [InlineData("", null)]
        public void When_Verifying_Hash_If_Password_IsNull_ThrowArgumentNullException(string password, string passwordHash)
        {
            Action action = () => service.VerifyPassword(password, passwordHash);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Calculate_Password_Hash_Correctly()
        {
            var password = "Apples";

            var passwordHash = service.GetPasswordHash(password);

            service.VerifyPassword(password, passwordHash).Should().BeTrue();
        }

        [Fact]
        public void Generate_Different_Password_Hash_Each_Time()
        {
            var password = "Apples";

            var passwordHashOne = service.GetPasswordHash(password);
            var passwordHashTwo = service.GetPasswordHash(password);

            passwordHashOne.Should().NotBe(passwordHashTwo);
        }
    }
}
