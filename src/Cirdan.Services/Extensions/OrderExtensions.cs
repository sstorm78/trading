using Cirdan.Repositories.Entities;

namespace Cirdan.Services.Extensions
{
    public static class OrderExtensions
    {
        public static Cirdan.Repositories.Entities.Order ToEntity(this Models.Order orderModel)
        {
            var result = new Cirdan.Repositories.Entities.Order
                   {
                       Timestamp = orderModel.Timestamp,
                       OrderId = orderModel.OrderId,
                       Ticker = orderModel.Ticker,
                       Side = SideExtensions.ToString(orderModel.Side),
                       Price = orderModel.Price,
                       Size = orderModel.Size
                   };

            return result;
        }

        public static Models.Order ToModel(this Order orderEntity)
        {
            return new Models.Order(
                orderEntity.Timestamp,
                orderEntity.OrderId,
                orderEntity.Ticker,
                orderEntity.Side.ToSideEnum(),
                orderEntity.Price,
                orderEntity.Size);
        }
    }
}
