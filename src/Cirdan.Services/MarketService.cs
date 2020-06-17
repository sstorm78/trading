using System;
using System.Linq;
using System.Threading.Tasks;
using Cirdan.Models;
using Cirdan.Repositories;
using Cirdan.Services.Extensions;

namespace Cirdan.Services
{
    public class MarketService
    {
        private IOrderBookRepository _orderBookRepository;
        private readonly IOrderValidationService _orderValidationService;

        public MarketService(IOrderBookRepository orderBookRepository,
            IOrderValidationService orderValidationService)
        {
            _orderBookRepository = orderBookRepository;
            _orderValidationService = orderValidationService;
        }

        public async Task ProcessOrder(string orderBookName, string order)
        {
            var orderDetails = _orderValidationService.ParseInputAndConvertToEntity(order);

            switch (orderDetails.Action)
            {
                case OrderAction.Add:
                    await _orderBookRepository.Add(orderBookName, orderDetails.ToEntity());
                    break;
                case OrderAction.Update:
                    await _orderBookRepository.Update(orderBookName, orderDetails.OrderId, orderDetails.Size);
                    break;
                case OrderAction.Cancel:
                    await _orderBookRepository.Cancel(orderBookName, orderDetails.OrderId);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(OrderAction));
            }
        }

        public async Task<BestBidAndAskResult> GetBestBidAndAsk(string orderBookName, string ticker)
        {
            var orderBook = await _orderBookRepository.GetOrderBookByName(orderBookName);

            if (orderBook == null)
            {
                return new BestBidAndAskResult(null,null);
            }

            var bestAsk = orderBook.Where(o => o.Side == Side.Ask.ToString()).OrderBy(o => o.Price).FirstOrDefault();
            var bestBid = orderBook.Where(o => o.Side == Side.Bid.ToString()).OrderByDescending(o => o.Price).FirstOrDefault();
            
            return new BestBidAndAskResult(bestBid?.ToModel(), bestAsk?.ToModel());
        }

    }
}
