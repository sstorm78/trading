namespace Cirdan.Models
{
    public class BestBidAndAskResult
    {
        public Order BestBid { get; }
        public Order BestAsk { get; }

        public BestBidAndAskResult(Order bestBid, Order bestAsk)
        {
            BestBid = bestBid;
            BestAsk = bestAsk;
        }
    }
}
