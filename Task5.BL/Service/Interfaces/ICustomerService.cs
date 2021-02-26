using System;
using System.Collections.Generic;
using System.Text;
using Task5.BL.DTO;

namespace Task5.BL.Service.Interfaces
{
    public interface ICustomerService
    {
        IEnumerable<CustomerDTO> GetAll();
        CustomerDTO FindById(int id);
        void Create(CustomerDTO customerDTO);
        void Delete(int id);
        void Update(CustomerDTO customerDTO);
    }
}
