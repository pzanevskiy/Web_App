using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Task5.BL.DTO;
using Task5.Models.Customer;
using Task5.Models.Manager;
using Task5.Models.Order;
using Task5.Models.Product;

namespace Task5.Util
{
    public class MapperWebConfig
    {
        public static MapperConfiguration Configure()
        {
            var config = new MapperConfiguration
            (
                cfg =>
                {
                    cfg.CreateMap<ProductDTO, ProductViewModel>();
                    cfg.CreateMap<ProductViewModel, ProductDTO>();
                    cfg.CreateMap<CustomerDTO, CustomerViewModel>();
                    cfg.CreateMap<CustomerViewModel, CustomerDTO>();
                    cfg.CreateMap<ManagerDTO, ManagerViewModel>();
                    cfg.CreateMap<ManagerViewModel, ManagerDTO>();
                    cfg.CreateMap<OrderDTO, OrderViewModel>()
                    .ForMember("Customer", opt => opt.MapFrom(x => x.Customer))
                    .ForMember("Product", opt => opt.MapFrom(x => x.Product))
                    .ForMember("Manager", opt => opt.MapFrom(x => x.Manager));
                    cfg.CreateMap<OrderViewModel, OrderDTO>();
                    cfg.CreateMap<CreateOrderViewModel, OrderDTO>();
                }
            );
            return config;
        }
    }
}