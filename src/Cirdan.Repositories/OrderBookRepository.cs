using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cirdan.Config;
using Cirdan.Repositories.Entities;

namespace Cirdan.Repositories
{
    public class OrderBookRepository : IOrderBookRepository
    {
        private Dictionary<string, List<Order>> _orderBooks;

        public OrderBookRepository()
        {
            _orderBooks = new Dictionary<string, List<Order>>();
        }

        public async Task Add(string orderBookName, Order order)
        {
            var _lock = new object();

            lock (_lock)
            {
                if (!_orderBooks.ContainsKey(orderBookName))
                {
                    _orderBooks.Add(orderBookName, new List<Order>());
                }

                if (_orderBooks[orderBookName].Any(i => i.OrderId == order.OrderId))
                {
                    throw new Exception(ExceptionMessages.OrderAlreadyExists);
                }

                _orderBooks[orderBookName].Add(order);
            }
        }

        public async Task Update(string orderBookName, string orderId, int size)
        {
            var _lock = new object();

            lock (_lock)
            {
                if (!_orderBooks.ContainsKey(orderBookName))
                {
                    throw new Exception(ExceptionMessages.OrderBookDoesNotExists);
                }

                if (_orderBooks[orderBookName].All(i => i.OrderId != orderId))
                {
                    throw new Exception(ExceptionMessages.OrderDoesNotExist);
                }

                var order = _orderBooks[orderBookName].First(i => i.OrderId == orderId);

                order.Size = size;
            }
        }

        public async Task Cancel(string orderBookName, string orderId)
        {
            var _lock = new object();

            lock (_lock)
            {
                if (!_orderBooks.ContainsKey(orderBookName))
                {
                    throw new Exception(ExceptionMessages.OrderBookDoesNotExists);
                }

                if (_orderBooks[orderBookName].All(i => i.OrderId != orderId))
                {
                    return;
                }

                var order = _orderBooks[orderBookName].First(i => i.OrderId == orderId);

                _orderBooks[orderBookName].Remove(order);
            }
        }

        public async Task<List<Order>> GetOrderBookByName(string orderBookName)
        {
            var _lock = new object();

            lock (_lock)
            {
                return !_orderBooks.ContainsKey(orderBookName) ? null : _orderBooks[orderBookName];
            }
        }
    }
}
