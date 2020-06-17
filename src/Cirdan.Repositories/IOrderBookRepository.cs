using System.Collections.Generic;
using System.Threading.Tasks;
using Cirdan.Repositories.Entities;

namespace Cirdan.Repositories
{
    public interface IOrderBookRepository
    {
        Task Add(string orderBookName, Order order);
        Task Update(string orderBookName, string orderId, int size);
        Task Cancel(string orderBookName, string orderId);

        Task<List<Order>> GetOrderBookByName(string orderBookName);
    }
}