using System;
using System.Collections.Generic;
using System.Text;
using Task5.BL.DTO;

namespace Task5.BL.Service.Interfaces
{
    public interface IManagerService
    {
        IEnumerable<ManagerDTO> GetAll();
        ManagerDTO FindById(int id);
        void Create(ManagerDTO managerDTO);
        void Delete(int id);
        void Update(ManagerDTO managerDTO);
    }
}
