using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Task5.BL.DTO;
using Task5.BL.Service.Interfaces;
using Task5.BL.Util;
using Task5.DAL.UnitOfWork.Interfaces;
using Task5.Entity;

namespace Task5.BL.Service
{
    public class ManagerService : IManagerService
    {
        private IUnitOfWork Database { get; set; }
        IMapper mapper;

        public ManagerService(IUnitOfWork uow)
        {
            Database = uow;
            mapper = new Mapper(MapperConfig.Configure());
        }

        public IEnumerable<ManagerDTO> GetAll()
        {
            return mapper.Map<IEnumerable<Manager>, IEnumerable<ManagerDTO>>(Database.Managers.Get());
        }

        public ManagerDTO FindById(int id)
        {
            return mapper.Map<Manager, ManagerDTO>(Database.Managers.Get(x => x.Id.Equals(id)));
        }

        public void Create(ManagerDTO managerDTO)
        {
            var manager = mapper.Map<ManagerDTO, Manager>(managerDTO);
            Database.Managers.Add(manager);
            Database.Save();
        }

        public void Delete(int id)
        {
            var manager = Database.Managers.Get(x => x.Id.Equals(id));
            Database.Managers.Delete(manager);
            Database.Save();
        }

        public void Update(ManagerDTO managerDTO)
        {
            var manager = mapper.Map<ManagerDTO, Manager>(managerDTO);
            Database.Managers.Update(manager);
            Database.Save();
        }
    }
}
