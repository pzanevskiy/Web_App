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
using Task5.Models.Order;
using Task5.Util;

namespace Task5.Controllers
{
    public class CustomerController : Controller
    {
        private IUnitOfWork _uow;
        private IMapper _mapper;
        private ICustomerService _customerService;
        private IOrderService _orderService;

        public CustomerController()
        {
            _uow = new EFUnitOfWork();
            _mapper = new Mapper(MapperWebConfig.Configure());
            _customerService = new CustomerService(_uow);
            _orderService = new OrderService(_uow);
        }

        public ActionResult Index(int? page)
        {
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
            catch (Exception e)
            {
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
                    return PartialView("List", customers.ToPagedList(1, customers.Count() == 0 ? 1 : customers.Count()));

                }
                return View();
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }


        public ActionResult Details(int id)
        {
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
                    _customerService.Create(_mapper.Map<CustomerViewModel, CustomerDTO>(model));
                    return RedirectToAction("Index", new { page = page });
                }
                return View();
            }
            catch
            {
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
                    _customerService.Update(_mapper.Map<CustomerViewModel, CustomerDTO>(model));
                    return RedirectToAction("Index", new { page = page });
                }
                return View();
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
            return View(_mapper.Map<CustomerDTO, CustomerViewModel>(_customerService.FindById(id)));
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id, FormCollection formCollection, int? page)
        {
            try
            {
                _customerService.Delete(id);
                return RedirectToAction("Index", new { page = page });
            }
            catch
            {
                return View();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _customerService != null)
            {
                _customerService.Dispose();
                _orderService.Dispose();
                _customerService = null;
                _orderService = null;                
            }
            base.Dispose(disposing);
        }
    }
}
