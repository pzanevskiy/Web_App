using Ninject.Modules;
using Task5.BL.Service;
using Task5.BL.Service.Interfaces;

namespace Task5.Util
{
    public class ModuleWeb : NinjectModule
    {
        public override void Load()
        {
            Bind<IOrderService>().To<OrderService>();
            Bind<IProductService>().To<ProductService>();
            Bind<ICustomerService>().To<CustomerService>();
            Bind<IManagerService>().To<ManagerService>();
        }        
    }
}