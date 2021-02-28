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
using Task5.Models.Order;
using Task5.Util;

namespace Task5.Controllers
{
    public class CustomerController : Controller
    {
        private IMapper _mapper;
        private ICustomerService _customerService;
        private IOrderService _orderService;
        private ILogger _logger;
        public CustomerController(ICustomerService customerSerivice, IOrderService orderService,ILogger logger)
        {
            _mapper = new Mapper(MapperWebConfig.Configure());
            _customerService = customerSerivice;
            _orderService = orderService;
            _logger = logger;
        }

        public ActionResult Index(int? page)
        {
            _logger.Info($"{User.Identity.Name} Hello customer index");
            ViewBag.CurrentPage = page ?? 1;
            return View();
        }

        public ActionResult Customers(int? page)
        {
            try
            {
                var customers = _mapper.Map<IEnumerable<CustomerDTO>, IEnumerable<CustomerViewModel>>(_customerService.GetAll());
                ViewBag.CurrentPage = page;
                return PartialView("List", customers.ToPagedList(page ?? 1, 3));
            }
            catch
            {
                _logger.Warn($"{User.Identity.Name} Customers error");
                return View("Error");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Customers(CustomerFilter model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var customers = _mapper.Map<IEnumerable<CustomerDTO>, IEnumerable<CustomerViewModel>>(_customerService.GetAll());
                    if (model.NickName != null)
                    {
                        customers = customers.Where(x => x.Nickname.ToLower().Contains(model.NickName.ToLower()));
                    }
                    if (model.PhoneNumber != null)
                    {
                        customers = customers.Where(x => x.PhoneNumber.ToLower().Contains(model.PhoneNumber.ToLower()));
                    }
                    _logger.Info($"{User.Identity.Name} Filter customers:\t Nickname: {model.NickName}, Phone: {model.PhoneNumber}");
                    return PartialView("List", customers.ToPagedList(1, customers.Count() == 0 ? 1 : customers.Count()));

                }
                _logger.Warn($"{User.Identity.Name} Customers filter error");
                return View();
            }
            catch
            {
                _logger.Warn($"{User.Identity.Name} Customers filter error");
                return View("Error");
            }
        }


        public ActionResult Details(int id)
        {
            _logger.Info($"{User.Identity.Name} Details customer");
            return PartialView(_mapper.Map<IEnumerable<OrderDTO>, IEnumerable<OrderViewModel>>(_orderService.GetOrdersByCustomerId(id)));
        }

        [Authorize(Roles = "admin")]
        public ActionResult Create(int? page)
        {
            var model = new CustomerViewModel();
            ViewBag.CurrentPage = page;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Create(CustomerViewModel model, int? page)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.Info($"{User.Identity.Name} create customer");
                    _customerService.Create(_mapper.Map<CustomerViewModel, CustomerDTO>(model));
                    return RedirectToAction("Index", new { page = page });
                }
                _logger.Warn($"{User.Identity.Name} Customers create error");
                return View();
            }
            catch
            {
                _logger.Warn($"{User.Identity.Name} Customers create error");
                return View();
            }
        }

        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id, int? page)
        {
            ViewBag.CurrentPage = page;
            return View(_mapper.Map<CustomerDTO, CustomerViewModel>(_customerService.FindById(id)));
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(CustomerViewModel model, int? page)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.Info($"{User.Identity.Name} edit customer");
                    _customerService.Update(_mapper.Map<CustomerViewModel, CustomerDTO>(model));
                    return RedirectToAction("Index", new { page = page });
                }
                _logger.Warn($"{User.Identity.Name} Customers edit error");
                return View();
            }
            catch
            {
                _logger.Warn($"{User.Identity.Name} Customers edit error");
                return View();
            }
        }

        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id, int? page)
        {
            ViewBag.CurrentPage = page;
            return View(_mapper.Map<CustomerDTO, CustomerViewModel>(_customerService.FindById(id)));
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id, FormCollection formCollection, int? page)
        {
            try
            {
                _logger.Info($"{User.Identity.Name} delete customer");
                _customerService.Delete(id);
                return RedirectToAction("Index", new { page = page });
            }
            catch
            {
                _logger.Warn($"{User.Identity.Name} Customers delete error");
                return View();
            }
        }
    }
}
