using System;
using System.Threading.Tasks;
using Cirdan.Config;
using Cirdan.Repositories;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Order = Cirdan.Repositories.Entities.Order;

namespace Cirdan.Services.Tests
{
    public class MarketServiceShould
    {

        [Test]
        public async Task ProcessOrder_ShouldSuccessfullyAddAnOrder()
        {
            var repoMock = new Mock<IOrderBookRepository>();
            var validator = new OrderValidationService();

            var sut = new MarketService(repoMock.Object, validator);

            await sut.ProcessOrder("test", "1568390243|abbb11|a|AAPL|B|209.00000|100");

            repoMock.Verify(a => a.Add(It.IsAny<string>(), It.Is<Order>(i => 
                                                                            i.Timestamp == 1568390243
                                                                            && i.OrderId == "abbb11"
                                                                            && i.Ticker == "AAPL"
                                                                            && i.Side == "b"
                                                                            && i.Price == 209.00000m
                                                                            && i.Size == 100)), Times.Once);
        }

        [Test]
        public async Task ProcessOrder_ShouldThrowAnExceptionWhenAddingAnOrderThatAlreadyBeenAdded()
        {
            var repo = new OrderBookRepository();
            var validator = new OrderValidationService();

            var sut = new MarketService(repo, validator);

            await sut.ProcessOrder("test", "1568390243|abbb11|a|AAPL|B|209.00000|100");

            Func<Task> test = async () => { await sut.ProcessOrder("test", "1568390243|abbb11|a|AAPL|B|209.00000|100"); };

            test.Should().Throw<Exception>().WithMessage(ExceptionMessages.OrderAlreadyExists);
        }

        [Test]
        public async Task ProcessOrder_ShouldSuccessfullyUpdateAnOrder()
        {
            var repoMock = new Mock<IOrderBookRepository>();
            var validator = new OrderValidationService();

            var sut = new MarketService(repoMock.Object, validator);

            await sut.ProcessOrder("test", "1568390243|abbb11|u|100");

            repoMock.Verify(a => a.Update(It.IsAny<string>(), "abbb11",100), Times.Once);
        }

        [Test]
        public async Task ProcessOrder_ShouldThrowAnExceptionWhenUpdatingButOrderWasNotFound()
        {
            var repo = new OrderBookRepository();
            var validator = new OrderValidationService();

            var sut = new MarketService(repo, validator);

            await sut.ProcessOrder("test", "1568390243|abbb11|a|AAPL|B|209.00000|100");

            Func<Task> test = async () => { await sut.ProcessOrder("test", "1568390243|abbb33|u|100"); };

            test.Should().Throw<Exception>().WithMessage(ExceptionMessages.OrderDoesNotExist);
        }

        [Test]
        public async Task ProcessOrder_ShouldSuccessfullyCancelAnOrder()
        {
            var repoMock = new Mock<IOrderBookRepository>();
            var validator = new OrderValidationService();

            var sut = new MarketService(repoMock.Object, validator);

            await sut.ProcessOrder("test", "1568390243|abbb11|c");

            repoMock.Verify(a => a.Cancel(It.IsAny<string>(), "abbb11"), Times.Once);
        }
    }
}