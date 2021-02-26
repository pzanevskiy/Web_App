using AutoMapper;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Task5.BL.DTO;
using Task5.BL.Service;
using Task5.BL.Service.Interfaces;
using Task5.DAL.UnitOfWork;
using Task5.DAL.UnitOfWork.Interfaces;
using Task5.Models.Customer;
using Task5.Models.Filters;
using Task5.Models.Manager;
using Task5.Models.Order;
using Task5.Models.Product;
using Task5.Util;

namespace Task5.Controllers
{
    public class OrderController : Controller
    {
        IUnitOfWork uow;
        IMapper mapper;
        IOrderService orderService;
        ICustomerService customerService;
        IManagerService managerService;
        IProductService productService;

        public OrderController()
        {
            uow = new EFUnitOfWork();
            mapper = new Mapper(MapperWebConfig.Configure());
            orderService = new OrderService(uow);
            customerService = new CustomerService(uow);
            managerService = new ManagerService(uow);
            productService = new ProductService(uow);
        }
        // GET: Order
        public ActionResult Index(int? page)
        {
            ViewBag.CurrentPage = page ?? 1;
            return View();
        }

        public ActionResult Orders(int? page)
        {
            try
            {
                var orders = mapper.Map<IEnumerable<OrderDTO>, IEnumerable<OrderViewModel>>(orderService.GetAll());
                ViewBag.CurrentPage = page;
                return PartialView("List", orders.ToPagedList(page ?? 1, 3));
            }
            catch (Exception e)
            {
                return View("Error");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Orders(OrderFilter model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var sales = mapper.Map<IEnumerable<OrderDTO>, IEnumerable<OrderViewModel>>(orderService.GetAll());
                    if (model.Date != null)
                    {
                        sales = sales.Where(x => (x.Date.Equals(model.Date)));
                    }
                    if (model.Customer != null)
                    {
                        sales = sales.Where(x => x.Customer.ToLower().Contains(model.Customer.ToLower()));
                    }
                    if (model.Product != null)
                    {
                        sales = sales.Where(x => x.Product.ToLower().Contains(model.Product.ToLower()));
                    }
                    if (model.Manager != null)
                    {
                        sales = sales.Where(x => x.Manager.ToLower().Contains(model.Manager.ToLower()));
                    }
                    return PartialView("List", sales.ToPagedList(1, sales.Count() == 0 ? 1 : sales.Count()));

                }
                return View();
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }
        // GET: Order/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            return PartialView(mapper.Map<OrderDTO, OrderViewModel>(orderService.FindById(id)));
        }

        // GET: Order/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create(int? page)
        {
            var model = new CreateOrderViewModel()
            {
                Customers = new SelectList(mapper.Map<IEnumerable<CustomerDTO>, IEnumerable<CustomerViewModel>>(customerService.GetAll()), "Nickname", "Nickname"),
                Products = new SelectList(mapper.Map<IEnumerable<ProductDTO>, IEnumerable<ProductViewModel>>(productService.GetAll()), "Name", "Name"),
                Managers = new SelectList(mapper.Map<IEnumerable<ManagerDTO>, IEnumerable<ManagerViewModel>>(managerService.GetAll()), "LastName", "LastName")
            };
            ViewBag.CurrentPage = page;
            return View(model);
        }

        // POST: Order/Create
        [HttpPost]
        public ActionResult Create(CreateOrderViewModel model, int? page)
        {

            if (!ModelState.IsValid)
            {
                model.Customers = new SelectList(mapper.Map<IEnumerable<CustomerDTO>, IEnumerable<CustomerViewModel>>(customerService.GetAll()), "Nickname", "Nickname");
                model.Products = new SelectList(mapper.Map<IEnumerable<ProductDTO>, IEnumerable<ProductViewModel>>(productService.GetAll()), "Name", "Name");
                model.Managers = new SelectList(mapper.Map<IEnumerable<ManagerDTO>, IEnumerable<ManagerViewModel>>(managerService.GetAll()), "FirstName", "FirstName");

                return View(model);
            }
            try
            {
                orderService.AddOrder(mapper.Map<CreateOrderViewModel, OrderDTO>(model));
                return RedirectToAction("Index", new { page = page });
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: Order/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id, int? page)
        {
            ViewBag.CurrentPage = page;
            return View(mapper.Map<OrderDTO, OrderViewModel>(orderService.FindById(id)));
        }

        // POST: Order/Edit/5
        [HttpPost]
        public ActionResult Edit(OrderViewModel model, int? page)
        {
            try
            {
                orderService.Update(mapper.Map<OrderViewModel, OrderDTO>(model));
                return RedirectToAction("Index", new { page = page });
            }
            catch
            {
                return View();
            }
        }

        // GET: Order/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id, int? page)
        {
            ViewBag.CurrentPage = page;
            return View(mapper.Map<OrderDTO, OrderViewModel>(orderService.FindById(id)));
        }

        // POST: Order/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection, int? page)
        {
            try
            {
                orderService.Delete(id);
                return RedirectToAction("Index", new { page = page });
            }
            catch
            {
                return View();
            }
        }
    }
}
