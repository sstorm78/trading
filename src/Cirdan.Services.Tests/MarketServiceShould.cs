using System;
using System.Threading.Tasks;
using Cirdan.Config;
using Cirdan.Repositories;
using Cirdan.Repositories.Entities;
using FluentAssertions;
using Moq;
using NUnit.Framework;

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

        [TestCase("c")]
        public void ProcessOrder_ShouldNotThrowExceptionIfCancellingAnOrderThanDoesNotExist(string action)
        {
            var repoMock = new Mock<IOrderBookRepository>();
            var validator = new OrderValidationService();
            var sut = new MarketService(repoMock.Object, validator);

            var payload = "1|v|c|4";

            Func<Task> test = async () => { await sut.ProcessOrder("test", "1568390243|abbb11|c"); };
            
            test.Should().NotThrow();
        }

        [Test]
        public async Task GetBestBidAndAsk_ShouldReturnNullsWhenNoOrderBookWasFound()
        {
            var repo = new OrderBookRepository();
            var validator = new OrderValidationService();
            
            var sut = new MarketService(repo, validator);

            var result = await sut.GetBestBidAndAsk("test", "aaa");

            result.Should().NotBeNull();
            result.BestAsk.Should().BeNull();
            result.BestBid.Should().BeNull();
        }

        [Test]
        public async Task GetBestBidAndAsk_ShouldReturnBestBidAndAskOrders()
        {
            var repo = new OrderBookRepository();
            var validator = new OrderValidationService();

            var sut = new MarketService(repo, validator);

            //add data
            await sut.ProcessOrder("test", "1568390242|abbb12|a|AAPL|B|100.00000|100");
            await sut.ProcessOrder("test", "1568390241|abbb11|a|AAPL|B|300.00000|100");
            await sut.ProcessOrder("test", "1568390243|abbb13|a|AAPL|B|200.00000|100");

            await sut.ProcessOrder("test", "1568390245|abbb15|a|AAPL|S|800.00000|100");
            await sut.ProcessOrder("test", "1568390246|abbb16|a|AAPL|S|700.00000|100");
            await sut.ProcessOrder("test", "1568390247|abbb17|a|AAPL|S|600.00000|100");

            var result = await sut.GetBestBidAndAsk("test", "AAPL");

            result.Should().NotBeNull();

            result.BestAsk.Should().NotBeNull();
            result.BestAsk.Price.Should().Be(600.00000m);

            result.BestBid.Should().NotBeNull();
            result.BestBid.Price.Should().Be(300.00000m);
        }
    }
}