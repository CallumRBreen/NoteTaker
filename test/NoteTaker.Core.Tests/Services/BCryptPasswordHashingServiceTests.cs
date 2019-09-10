using System;
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
            Assert.Throws<ArgumentException>(() => service.GetPasswordHash(password));
        }

        [Theory]
        [InlineData(null,"")]
        [InlineData("", null)]
        public void When_Verifying_Hash_If_Password_IsNull_ThrowArgumentNullException(string password, string passwordHash)
        {
            Assert.Throws<ArgumentNullException>(() => service.VerifyPassword(password, passwordHash));
        }

        [Fact]
        public void Calculate_Password_Hash_Correctly()
        {
            var password = "Apples";

            var passwordHash = service.GetPasswordHash(password);

            Assert.True(service.VerifyPassword(password, passwordHash));
        }

        [Fact]
        public void Generate_Different_Password_Hash_Each_Time()
        {
            var password = "Apples";

            var passwordHashOne = service.GetPasswordHash(password);
            var passwordHashTwo = service.GetPasswordHash(password);

            Assert.NotEqual(passwordHashOne, passwordHashTwo);
        }
    }
}
