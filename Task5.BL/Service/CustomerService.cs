using AutoMapper;
using System;
using System.Collections.Generic;
using Task5.BL.DTO;
using Task5.BL.Service.Interfaces;
using Task5.BL.Util;
using Task5.DAL.UnitOfWork.Interfaces;
using Task5.DAL.UnitOfWork;
using Task5.Entity;

namespace Task5.BL.Service
{
    public class CustomerService : ICustomerService
    {
        private bool _disposed = false;
        private IMapper _mapper;

        private IUnitOfWork Database { get; set; }

        public CustomerService(IUnitOfWork uow)
        {
            Database = uow;
            _mapper = new Mapper(MapperConfig.Configure());
        }

        public IEnumerable<CustomerDTO> GetAll()
        {
            return _mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerDTO>>(Database.Customers.Get());
        }

        public CustomerDTO FindById(int id)
        {
            var customer = _mapper.Map<Customer, CustomerDTO>(Database.Customers.Get(x => x.Id.Equals(id)));
            return customer;
        }

        public void Create(CustomerDTO customerDTO)
        {
            var customer = _mapper.Map<CustomerDTO, Customer>(customerDTO);
            Database.Customers.Add(customer);
            Database.Save();
        }

        public void Delete(int id)
        {
            var customer = Database.Customers.Get(x => x.Id.Equals(id));
            Database.Customers.Delete(customer);
            Database.Save();
        }

        public void Update(CustomerDTO customerDTO)
        {
            var customer = _mapper.Map<CustomerDTO, Customer>(customerDTO);
            Database.Customers.Update(customer);
            Database.Save();
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

        ~CustomerService() => Dispose(false);
    }
}
