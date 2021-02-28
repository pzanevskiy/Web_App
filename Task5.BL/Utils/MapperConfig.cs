using AutoMapper;
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
                    cfg.CreateMap<Product, ProductDTO>().ReverseMap();
                    cfg.CreateMap<Customer, CustomerDTO>().ReverseMap();
                    cfg.CreateMap<Manager, ManagerDTO>().ReverseMap();
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
