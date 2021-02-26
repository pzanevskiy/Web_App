using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Task5.BL.DTO;
using Task5.Entity;

namespace Task5.BL.Util
{
    public class MapperConfig
    {
        public static MapperConfiguration Configure()
        {
            var config = new MapperConfiguration
            (
                cfg =>
                {
                    cfg.CreateMap<Product, ProductDTO>();
                    cfg.CreateMap<ProductDTO, Product>();
                    cfg.CreateMap<Customer, CustomerDTO>();
                    cfg.CreateMap<CustomerDTO, Customer>();
                    cfg.CreateMap<Manager, ManagerDTO>();
                    cfg.CreateMap<ManagerDTO, Manager>();
                    cfg.CreateMap<Order, OrderDTO>()
                    .ForMember("Customer", opt => opt.MapFrom(x => x.Customer.Nickname))
                    .ForMember("Product", opt => opt.MapFrom(x => x.Product.Name))
                    .ForMember("Manager", opt => opt.MapFrom(x => x.Manager.LastName));
                    cfg.CreateMap<OrderDTO, Order>();
                }
            );
            return config;
        }
    }
}
