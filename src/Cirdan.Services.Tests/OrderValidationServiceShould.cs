using System;
using Cirdan.Config;
using Cirdan.Models;
using FluentAssertions;
using NUnit.Framework;

namespace Cirdan.Services.Tests
{
    public class OrderValidationServiceShould
    {
        [Test]
        public void ParseInputAndConvertToEntity_ShouldThrowExceptionWhenInvalidDataPassed()
        {
            var sut = new OrderValidationService();

            var payload = "a";

            Action test = () => { sut.ParseInputAndConvertToEntity(payload); };

            test.Should().Throw<ValidationException>().WithMessage(Validation.InvalidOrderDetailsLength);
        }

        [Test]
        public void ParseInputAndConvertToEntity_ShouldThrowExceptionWhenInvalidTimestampPassed()
        {
            var sut = new OrderValidationService();

            var payload = "a|b|c";

            Action test = () => { sut.ParseInputAndConvertToEntity(payload); };

            test.Should().Throw<ValidationException>().WithMessage(Validation.InvalidOrderTimestamp);
        }

        [Test]
        public void ParseInputAndConvertToEntity_ShouldThrowExceptionWhenInvalidOrderIdPassed()
        {
            var sut = new OrderValidationService();

            var payload = "1||c";

            Action test = () => { sut.ParseInputAndConvertToEntity(payload); };

            test.Should().Throw<ValidationException>().WithMessage(Validation.InvalidOrderId);
        }


        [Test]
        public void ParseInputAndConvertToEntity_ShouldThrowExceptionWhenInvalidActionPassed()
        {
            var sut = new OrderValidationService();

            var payload = "1|v|d";

            Action test = () => { sut.ParseInputAndConvertToEntity(payload); };

            test.Should().Throw<ValidationException>().WithMessage(Validation.InvalidAction);
        }

        [Test]
        public void ParseInputAndConvertToEntity_ShouldThrowExceptionWhenInvalidTickerPassed()
        {
            var sut = new OrderValidationService();

            var payload = "1|v|a|";

            Action test = () => { sut.ParseInputAndConvertToEntity(payload); };

            test.Should().Throw<ValidationException>().WithMessage(Validation.InvalidTicker);
        }

        [Test]
        public void ParseInputAndConvertToEntity_ShouldThrowExceptionWhenInvalidSidePassed()
        {
            var sut = new OrderValidationService();

            var payload = "1|v|a|dd|w";

            Action test = () => { sut.ParseInputAndConvertToEntity(payload); };

            test.Should().Throw<ValidationException>().WithMessage(Validation.InvalidOrderSideValue);
        }

        [TestCase("")]
        [TestCase("0")]
        [TestCase("-1")]
        public void ParseInputAndConvertToEntity_ShouldThrowExceptionWhenInvalidPricePassed(string price)
        {
            var sut = new OrderValidationService();

            var payload = "1|v|a|dd|w|" + price;

            Action test = () => { sut.ParseInputAndConvertToEntity(payload); };

            test.Should().Throw<ValidationException>().WithMessage(Validation.InvalidOrderSideValue);
        }

        [TestCase("")]
        [TestCase("0")]
        [TestCase("-1")]
        public void ParseInputAndConvertToEntity_ShouldThrowExceptionWhenInvalidSizePassed(string size)
        {
            var sut = new OrderValidationService();

            var payload = "1|v|a|dd|w|5|" + size;

            Action test = () => { sut.ParseInputAndConvertToEntity(payload); };

            test.Should().Throw<ValidationException>().WithMessage(Validation.InvalidOrderSideValue);
        }

        [TestCase("a")]
        [TestCase("u")]
        [TestCase("c")]
        public void ParseInputAndConvertToEntity_ShouldNoThrowExceptionWhenValidActionsPassed(string action)
        {
            var sut = new OrderValidationService();

            var payload = "1|v|" + action + "|4|s|3|3";

            Action test = () => { sut.ParseInputAndConvertToEntity(payload); };

            test.Should().NotThrow();
        }

        [TestCase("b")]
        [TestCase("s")]
        public void ParseInputAndConvertToEntity_ShouldNoThrowExceptionWhenValidSidePassed(string side)
        {
            var sut = new OrderValidationService();

            var payload = "1|v|a|4|" + side + "|3|3";

            Action test = () => { sut.ParseInputAndConvertToEntity(payload); };

            test.Should().NotThrow();
        }
    }
}
