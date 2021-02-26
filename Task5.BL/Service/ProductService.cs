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
    public class ProductService : IProductService
    {
        private bool _disposed = false;
        private IMapper _mapper;

        private IUnitOfWork Database { get; set; }

        public ProductService(IUnitOfWork uow)
        {
            Database = uow;
            _mapper = new Mapper(MapperConfig.Configure());
        }

        public IEnumerable<ProductDTO> GetAll()
        {
            return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(Database.Products.Get());
        }

        public ProductDTO FindById(int id)
        {
            var product = _mapper.Map<Product, ProductDTO>(Database.Products.Get(x => x.Id.Equals(id)));
            return product;
        }

        public void Create(ProductDTO productDTO)
        {
            var product = _mapper.Map<ProductDTO, Product>(productDTO);
            Database.Products.Add(product);
            Database.Save();
        }

        public void Delete(int id)
        {
            var product = Database.Products.Get(x => x.Id.Equals(id));
            Database.Products.Delete(product);
            Database.Save();
        }

        public void Update(ProductDTO productDTO)
        {
            var product = _mapper.Map<ProductDTO, Product>(productDTO);
            Database.Products.Update(product);
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

        ~ProductService() => Dispose(false);
    }
}
