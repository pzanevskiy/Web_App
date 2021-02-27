using System;
using Task5.DAL.Context;
using Task5.DAL.Repositories;
using Task5.DAL.Repositories.Interfaces;
using Task5.DAL.UnitOfWork.Interfaces;
using Task5.Entity;

namespace Task5.DAL.UnitOfWork
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private SaleContext _context;
        private IGenericRepository<Customer> _customerRepository;
        private IGenericRepository<Order> _orderRepository;
        private IGenericRepository<Product> _productRepository;
        private IGenericRepository<Manager> _managerRepository;

        public EFUnitOfWork()
        {
            _context = new SaleContext();
        }

        public IGenericRepository<Customer> Customers
        {
            get
            {
                if (_customerRepository == null)
                {
                    _customerRepository = new GenericRepository<Customer>(_context);
                }
                return _customerRepository;
            }
        }

        public IGenericRepository<Product> Products
        {
            get
            {
                if (_productRepository == null)
                {
                    _productRepository = new GenericRepository<Product>(_context);
                }
                return _productRepository;
            }
        }

        public IGenericRepository<Order> Orders
        {
            get
            {
                if (_orderRepository == null)
                {
                    _orderRepository = new GenericRepository<Order>(_context);
                }
                return _orderRepository;
            }
        }

        public IGenericRepository<Manager> Managers
        {
            get
            {
                if (_managerRepository == null)
                {
                    _managerRepository = new GenericRepository<Manager>(_context);
                }
                return _managerRepository;
            }
        }
        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~EFUnitOfWork() => Dispose();
    }
}
