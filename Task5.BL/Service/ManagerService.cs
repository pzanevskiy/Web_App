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
    public class ManagerService : IManagerService
    {
        private bool _disposed = false;
        private IMapper _mapper;

        private IUnitOfWork Database { get; set; }

        public ManagerService(IUnitOfWork uow)
        {
            Database = uow;
            _mapper = new Mapper(MapperConfig.Configure());
        }

        public IEnumerable<ManagerDTO> GetAll()
        {
            return _mapper.Map<IEnumerable<Manager>, IEnumerable<ManagerDTO>>(Database.Managers.Get());
        }

        public ManagerDTO FindById(int id)
        {
            return _mapper.Map<Manager, ManagerDTO>(Database.Managers.Get(x => x.Id.Equals(id)));
        }

        public void Create(ManagerDTO managerDTO)
        {
            var manager = _mapper.Map<ManagerDTO, Manager>(managerDTO);
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
            var manager = _mapper.Map<ManagerDTO, Manager>(managerDTO);
            Database.Managers.Update(manager);
            Database.Save();
        }

        public object GetManagersWithOrdersCount()
        {
            return Database.Managers.Get().Select(x => new object[] { x.LastName, x.Order.Count }).ToArray();
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

        ~ManagerService() => Dispose(false);
    }
}
