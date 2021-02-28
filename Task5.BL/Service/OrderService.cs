using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task5.BL.DTO;
using Task5.BL.Service.Interfaces;
using Task5.BL.Util;
using Task5.DAL.UnitOfWork.Interfaces;
using Task5.Entity;

namespace Task5.BL.Service
{
    public class OrderService : IOrderService
    {
        private bool _disposed = false;
        private IMapper _mapper;

        private IUnitOfWork Database { get; set; }

        public OrderService(IUnitOfWork uow)
        {
            Database = uow;
            _mapper = new Mapper(MapperConfig.Configure());
        }

        public OrderDTO FindById(int id)
        {
            var order = _mapper.Map<Order, OrderDTO>(Database.Orders.Get(x => x.Id.Equals(id)));
            return order;
        }

        public void Update(OrderDTO orderDTO)
        {
            var order = Database.Orders.Get(x => x.Id.Equals(orderDTO.Id));
            order.Date = orderDTO.Date;
            order.Price = orderDTO.Price;
            order.Customer = Database.Customers.Get(x => x.Nickname.Equals(orderDTO.Customer));
            order.Product = Database.Products.Get(x => x.Name.Equals(orderDTO.Product));
            order.Manager = Database.Managers.Get(x => x.LastName.Equals(orderDTO.Manager));
            Database.Orders.Update(order);
            Database.Save();
        }

        public void Delete(int id)
        {
            var order = Database.Orders.Get(x => x.Id.Equals(id));
            Database.Orders.Delete(order);
            Database.Save();
        }

        public void AddOrder(OrderDTO orderDTO)
        {
            var cutomer = Database.Customers.Get(x => x.Nickname.Equals(orderDTO.Customer));
            var product = Database.Products.Get(x => x.Name.Equals(orderDTO.Product));
            var manager = Database.Managers.Get(x => x.LastName.Equals(orderDTO.Manager));
            var order = new Order()
            {
                Date = orderDTO.Date,
                Price = orderDTO.Price
            };

            if (cutomer != null)
            {
                order.Customer = cutomer;
            }

            if (product != null)
            {
                order.Product = product;
            }

            if (manager != null)
            {
                order.Manager = manager;
            }

            Database.Orders.Add(order);
            Database.Save();
        }

        public IEnumerable<OrderDTO> GetAll()
        {
            return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(Database.Orders.Get());
        }

        public IEnumerable<OrderDTO> GetOrdersByCustomerId(int id)
        {
            return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(Database.Customers.Get(x=>x.Id.Equals(id)).Orders);
        }

        public IEnumerable<OrderDTO> GetOrdersByManagerId(int id)
        {
            return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(Database.Managers.Get(x=>x.Id.Equals(id)).Order);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Database.Dispose();
                }
                _disposed = true;
            }
        }

        ~OrderService() => Dispose(false);
       

    }
}
