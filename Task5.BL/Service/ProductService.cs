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
        private IUnitOfWork Database { get; set; }
        IMapper mapper;

        public ProductService(IUnitOfWork uow)
        {
            Database = uow;
            mapper = new Mapper(MapperConfig.Configure());
        }

        public IEnumerable<ProductDTO> GetAll()
        {
            return mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(Database.Products.Get());
        }

        public ProductDTO FindById(int id)
        {
            var product = mapper.Map<Product, ProductDTO>(Database.Products.Get(x => x.Id.Equals(id)));
            return product;
        }

        public void Create(ProductDTO productDTO)
        {
            var product = mapper.Map<ProductDTO, Product>(productDTO);
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
            var product = mapper.Map<ProductDTO, Product>(productDTO);
            Database.Products.Update(product);
            Database.Save();
        }
    }
}
