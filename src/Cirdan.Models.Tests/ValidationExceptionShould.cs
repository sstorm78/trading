using FluentAssertions;
using NUnit.Framework;

namespace Cirdan.Models.Tests
{
    public class ValidationExceptionShould
    {
        [Test]
        public void Constructor_ShouldPopulateBaseMessage()
        {
            var result = new ValidationException("test");

            result.Message.Should().Be("test");
        }

    }
}
