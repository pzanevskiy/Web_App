using AutoMapper;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private IUnitOfWork _uow;
        private IMapper _mapper;
        private IOrderService _orderService;
        private ICustomerService _customerService;
        private IManagerService _managerService;
        private IProductService _productService;

        public OrderController()
        {
            _uow = new EFUnitOfWork();
            _mapper = new Mapper(MapperWebConfig.Configure());
            _orderService = new OrderService(_uow);
            _customerService = new CustomerService(_uow);
            _managerService = new ManagerService(_uow);
            _productService = new ProductService(_uow);
        }

        public ActionResult Index(int? page)
        {
            ViewBag.CurrentPage = page ?? 1;
            return View();
        }

        public ActionResult Orders(int? page)
        {
            try
            {
                var orders = _mapper.Map<IEnumerable<OrderDTO>, IEnumerable<OrderViewModel>>(_orderService.GetAll());
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
                    var sales = _mapper.Map<IEnumerable<OrderDTO>, IEnumerable<OrderViewModel>>(_orderService.GetAll());
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

        [Authorize]
        public ActionResult Details(int id)
        {
            return PartialView(_mapper.Map<OrderDTO, OrderViewModel>(_orderService.FindById(id)));
        }

        [Authorize(Roles = "admin")]
        public ActionResult Create(int? page)
        {
            var model = new CreateOrderViewModel()
            {
                Customers = new SelectList(_mapper.Map<IEnumerable<CustomerDTO>, IEnumerable<CustomerViewModel>>(_customerService.GetAll()), "Nickname", "Nickname"),
                Products = new SelectList(_mapper.Map<IEnumerable<ProductDTO>, IEnumerable<ProductViewModel>>(_productService.GetAll()), "Name", "Name"),
                Managers = new SelectList(_mapper.Map<IEnumerable<ManagerDTO>, IEnumerable<ManagerViewModel>>(_managerService.GetAll()), "LastName", "LastName")
            };
            ViewBag.CurrentPage = page;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreateOrderViewModel model, int? page)
        {

            if (!ModelState.IsValid)
            {
                model.Customers = new SelectList(_mapper.Map<IEnumerable<CustomerDTO>, IEnumerable<CustomerViewModel>>(_customerService.GetAll()), "Nickname", "Nickname");
                model.Products = new SelectList(_mapper.Map<IEnumerable<ProductDTO>, IEnumerable<ProductViewModel>>(_productService.GetAll()), "Name", "Name");
                model.Managers = new SelectList(_mapper.Map<IEnumerable<ManagerDTO>, IEnumerable<ManagerViewModel>>(_managerService.GetAll()), "LastName", "LastName");

                return View(model);
            }
            try
            {
                _orderService.AddOrder(_mapper.Map<CreateOrderViewModel, OrderDTO>(model));
                return RedirectToAction("Index", new { page = page });
            }
            catch
            {
                return View("Error");
            }
        }

        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id, int? page)
        {
            ViewBag.CurrentPage = page;
            return View(_mapper.Map<OrderDTO, OrderViewModel>(_orderService.FindById(id)));
        }

        [HttpPost]
        public ActionResult Edit(OrderViewModel model, int? page)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _orderService.Update(_mapper.Map<OrderViewModel, OrderDTO>(model));
                    return RedirectToAction("Index", new { page = page });
                }return View();
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id, int? page)
        {
            ViewBag.CurrentPage = page;
            return View(_mapper.Map<OrderDTO, OrderViewModel>(_orderService.FindById(id)));
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection, int? page)
        {
            try
            {
                _orderService.Delete(id);
                return RedirectToAction("Index", new { page = page });
            }
            catch
            {
                return View();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _orderService != null)
            {
                _managerService.Dispose();
                _orderService.Dispose();
                _customerService.Dispose();
                _productService.Dispose();
                _managerService = null;
                _orderService = null;
                _customerService = null;
                _productService = null;
            }
            base.Dispose(disposing);
        }
    }
}
