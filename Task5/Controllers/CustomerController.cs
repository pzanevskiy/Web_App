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
using Task5.Models.Order;
using Task5.Util;

namespace Task5.Controllers
{
    public class CustomerController : Controller
    {
        IUnitOfWork uow;
        IMapper mapper;
        ICustomerService customerService;
        IOrderService orderService;

        public CustomerController()
        {
            uow = new EFUnitOfWork();
            mapper = new Mapper(MapperWebConfig.Configure());
            customerService = new CustomerService(uow);
            orderService = new OrderService(uow);
        }
        // GET: Customer
        public ActionResult Index(int? page)
        {
            ViewBag.CurrentPage = page ?? 1;
            return View();
        }

        public ActionResult Customers(int? page)
        {
            try
            {
                var customers = mapper.Map<IEnumerable<CustomerDTO>, IEnumerable<CustomerViewModel>>(customerService.GetAll());
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
                    var customers = mapper.Map<IEnumerable<CustomerDTO>, IEnumerable<CustomerViewModel>>(customerService.GetAll());
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


        // GET: Customer/Details/5
        public ActionResult Details(int id)
        {
            return PartialView(mapper.Map<IEnumerable<OrderDTO>, IEnumerable<OrderViewModel>>(orderService.GetOrdersByCustomerId(id)));
        }

        // GET: Customer/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create(int? page)
        {
            var model = new CustomerViewModel();
            ViewBag.CurrentPage = page;
            return View(model);
        }

        // POST: Customer/Create
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Create(CustomerViewModel model, int? page)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    customerService.Create(mapper.Map<CustomerViewModel, CustomerDTO>(model));
                    return RedirectToAction("Index", new { page = page });
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Customer/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id, int? page)
        {
            ViewBag.CurrentPage = page;
            return View(mapper.Map<CustomerDTO, CustomerViewModel>(customerService.FindById(id)));
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(CustomerViewModel model, int? page)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    customerService.Update(mapper.Map<CustomerViewModel, CustomerDTO>(model));
                    return RedirectToAction("Index", new { page = page });
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Customer/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id, int? page)
        {
            ViewBag.CurrentPage = page;
            return View(mapper.Map<CustomerDTO, CustomerViewModel>(customerService.FindById(id)));
        }

        // POST: Customer/Delete/5
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id, FormCollection formCollection, int? page)
        {
            try
            {
                customerService.Delete(id);
                return RedirectToAction("Index", new { page = page });
            }
            catch
            {
                return View();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && customerService != null)
            {
                customerService.Dispose();
                orderService.Dispose();
                customerService = null;
                orderService = null;                
            }
            base.Dispose(disposing);
        }
    }
}
