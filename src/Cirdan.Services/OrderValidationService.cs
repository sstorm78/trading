using Cirdan.Config;
using Cirdan.Models;
using Cirdan.Services.Extensions;
using Order = Cirdan.Models.Order;

namespace Cirdan.Services
{
    public class OrderValidationService : IOrderValidationService
    {
        public Order ParseInputAndConvertToEntity(string orderDetails)
        {
            var splitData = orderDetails.Split(Parser.Delimeter);

            if (splitData.Length < Validation.MinimumDetailsLength)
            {
                throw new ValidationException(Validation.InvalidOrderDetailsLength);
            }

            if (!long.TryParse(splitData[Parser.TimestampIndex], out var timestamp))
            {
                throw new ValidationException(Validation.InvalidOrderTimestamp);
            }

            if (string.IsNullOrEmpty(splitData[Parser.OrderIdIndex]))
            {
                throw new ValidationException(Validation.InvalidOrderId);
            }

            var orderId = splitData[Parser.OrderIdIndex];

            OrderAction action;

            switch (splitData[Parser.ActionIndex].ToLowerInvariant())
            {
                case "a":
                    action = OrderAction.Add;
                    break;
                case "u":
                    return ValidateUpdate(splitData);
                case "c":
                    action = OrderAction.Cancel;
                    return new Order(timestamp, orderId, action);
                default:
                    throw new ValidationException(Validation.InvalidAction);
            }
            
            if (string.IsNullOrEmpty(splitData[Parser.TickerIndex]))
            {
                throw new ValidationException(Validation.InvalidTicker);
            }

            var ticker = splitData[Parser.TickerIndex];

            var side = splitData[Parser.SideIndex].ToLowerInvariant().ToSideEnum();
            
            if (!decimal.TryParse(splitData[Parser.PriceIndex], out var price))
            {
                throw new ValidationException(Validation.InvalidPrice);
            }

            if (price <= 0)
            {
                throw new ValidationException(Validation.InvalidPrice);
            }

            if (!int.TryParse(splitData[Parser.SizeIndex], out var size))
            {
                throw new ValidationException(Validation.InvalidSize);
            }

            if (size <= 0)
            {
                throw new ValidationException(Validation.InvalidSize);
            }

            return new Order(timestamp, orderId, action, ticker, side, price, size);
        }

        private Order ValidateUpdate(string[] splitData)
        {
            if (!int.TryParse(splitData[Parser.UpdateSizeIndex], out var size))
            {
                throw new ValidationException(Validation.InvalidSize);
            }

            if (size <= 0)
            {
                throw new ValidationException(Validation.InvalidSize);
            }

            var timeStamp = long.Parse(splitData[Parser.TimestampIndex]);
            var orderId = splitData[Parser.OrderIdIndex];

            return new Order(timeStamp, orderId, OrderAction.Update, size);
        }
    }
}
