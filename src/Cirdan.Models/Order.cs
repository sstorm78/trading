namespace Cirdan.Models
{
    public class Order
    {
        public string OrderId { get; }
        public long Timestamp { get;  }
        public string Ticker { get;  }
        public Side Side { get; }
        public OrderAction Action { get; }
        public decimal Price { get; }
        public int Size { get;  }

        public Order(long timestamp, string orderId, OrderAction action)
        {
            Timestamp = timestamp;
            OrderId = orderId;
            Action = action;
        }

        public Order(long timestamp, string orderId, OrderAction action, int size)
        {
            Timestamp = timestamp;
            OrderId = orderId;
            Action = action;
            Size = size;
        }

        public Order(long timestamp, string orderId, OrderAction action, string ticker, Side side, decimal price, int size)
        {
            Timestamp = timestamp;
            OrderId = orderId;
            Action = action;
            Ticker = ticker;
            Side = side;
            Price = price;
            Size = size;
        }

        public Order(long timestamp, string orderId, string ticker, Side side, decimal price, int size)
        {
            Timestamp = timestamp;
            OrderId = orderId;
            Ticker = ticker;
            Side = side;
            Price = price;
            Size = size;
        }
    }
}
