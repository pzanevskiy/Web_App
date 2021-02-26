using System;
using System.Collections.Generic;
using System.Text;
using Task5.BL.DTO;

namespace Task5.BL.Service.Interfaces
{
    public interface IOrderService : IDisposable
    {
        void AddOrder(OrderDTO orderDTO);
        IEnumerable<OrderDTO> GetAll();
        OrderDTO FindById(int id);
        void Delete(int id);
        void Update(OrderDTO orderDTO);
        IEnumerable<OrderDTO> GetOrdersByCustomerId(int id);
        IEnumerable<OrderDTO> GetOrdersByManagerId(int id);
    }
}
