using AutoMapper;
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
                    cfg.CreateMap<ProductDTO, ProductViewModel>().ReverseMap();
                    cfg.CreateMap<CustomerDTO, CustomerViewModel>().ReverseMap();
                    cfg.CreateMap<ManagerDTO, ManagerViewModel>().ReverseMap();
                    cfg.CreateMap<OrderDTO, OrderViewModel>()
                    .ForMember("Customer", opt => opt.MapFrom(x => x.Customer))
                    .ForMember("Product", opt => opt.MapFrom(x => x.Product))
                    .ForMember("Manager", opt => opt.MapFrom(x => x.Manager));
                    cfg.CreateMap<OrderViewModel, OrderDTO>();
                    cfg.CreateMap<CreateOrderViewModel, OrderDTO>().ReverseMap();
                }
            );
            return config;
        }
    }
}