using System;
using System.Collections.Generic;
using System.Text;
using Task5.DAL.Repositories.Interfaces;
using Task5.Entity;

namespace Task5.DAL.UnitOfWork.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Customer> Customers { get; }
        IGenericRepository<Product> Products { get; }
        IGenericRepository<Order> Orders { get; }
        IGenericRepository<Manager> Managers { get; }

        void Save();
    }
}
