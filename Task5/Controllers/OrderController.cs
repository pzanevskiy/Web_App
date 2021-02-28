using AutoMapper;
using Ninject.Extensions.Logging;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Task5.BL.DTO;
using Task5.BL.Service.Interfaces;
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
        private IMapper _mapper;
        private IOrderService _orderService;
        private ICustomerService _customerService;
        private IManagerService _managerService;
        private IProductService _productService;
        private ILogger _logger;

        public OrderController(IOrderService orderService, ICustomerService customerService, IManagerService managerService,
            IProductService productService, ILogger logger)
        {
            _mapper = new Mapper(MapperWebConfig.Configure());
            _orderService = orderService;
            _customerService = customerService;
            _managerService = managerService;
            _productService = productService;
            _logger = logger;
        }

        public ActionResult Index(int? page)
        {
            _logger.Info("Hello index orders");
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
            catch
            {
                _logger.Warn($"{User.Identity.Name} Orders error");
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
                    _logger.Info($"{User.Identity.Name} filter orders");
                    return PartialView("List", sales.ToPagedList(1, sales.Count() == 0 ? 1 : sales.Count()));

                }
                _logger.Warn($"{User.Identity.Name} Orders filter error");
                return View();
            }
            catch
            {
                _logger.Warn($"{User.Identity.Name} Orders filter error");
                return View("Error");
            }
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            _logger.Info($"{User.Identity.Name} filter details");
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

                _logger.Warn($"{User.Identity.Name} Orders create error");
                return View(model);
            }
            try
            {
                _logger.Info($"{User.Identity.Name} create order");
                _orderService.AddOrder(_mapper.Map<CreateOrderViewModel, OrderDTO>(model));
                return RedirectToAction("Index", new { page = page });
            }
            catch
            {
                _logger.Warn($"{User.Identity.Name} Orders create error");
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
                    _logger.Info($"{User} edit order");
                    _orderService.Update(_mapper.Map<OrderViewModel, OrderDTO>(model));
                    return RedirectToAction("Index", new { page = page });
                }
                _logger.Warn($"{User.Identity.Name} Orders edit error");
                return View();
            }
            catch
            {
                _logger.Warn($"{User.Identity.Name} Orders edit error");
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
                _logger.Info($"{User.Identity.Name} delete order");
                _orderService.Delete(id);
                return RedirectToAction("Index", new { page = page });
            }
            catch
            {
                _logger.Warn($"{User.Identity.Name} Orders delete error");
                return View();
            }
        }

        [HttpGet]
        public JsonResult GetChartData()
        {
            var item = _orderService
                .GetAll()
                .GroupBy(x => x.Date.ToString("d"))
                .Select(x => new object[] { x.Key.ToString(), x.Count() })
                .ToArray();
            return Json(item, JsonRequestBehavior.AllowGet);
        }
    }
}
