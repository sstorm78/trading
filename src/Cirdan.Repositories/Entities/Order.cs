namespace Cirdan.Repositories.Entities
{
    public class Order
    {
        public string OrderId { get; set; }
        public long Timestamp { get; set; }
        public string Ticker { get; set; }
        public string Side { get; set; }
        public decimal Price { get; set; }
        public int Size { get; set; }
    }
}
