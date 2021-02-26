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
using Task5.Models.Filters;
using Task5.Models.Manager;
using Task5.Models.Order;
using Task5.Util;

namespace Task5.Controllers
{
    public class ManagerController : Controller
    {
        IUnitOfWork uow;
        IMapper mapper;
        IManagerService managerService;
        IOrderService orderService;
        public ManagerController()
        {
            uow = new EFUnitOfWork();
            mapper = new Mapper(MapperWebConfig.Configure());
            managerService = new ManagerService(uow);
            orderService = new OrderService(uow);
        }

        // GET: Manager
        public ActionResult Index(int? page)
        {
            ViewBag.CurrentPage = page ?? 1;
            return View();
        }

        /*mapper.Map<IEnumerable<ManagerDTO>, IEnumerable<ManagerViewModel>>(managerService.GetAll())*/
        public ActionResult Managers(int? page)
        {
            try
            {
                var managers = mapper.Map<IEnumerable<ManagerDTO>, IEnumerable<ManagerViewModel>>(managerService.GetAll());
                ViewBag.CurrentPage = page;
                return PartialView("List", managers.ToPagedList(page ?? 1, 3));
            }
            catch (Exception e)
            {
                return View("Error");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Managers(ManagerFilter model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var managers = mapper.Map<IEnumerable<ManagerDTO>, IEnumerable<ManagerViewModel>>(managerService.GetAll());
                    if (model.LastName != null)
                    {
                        managers = managers.Where(x => x.LastName.ToLower().Contains(model.LastName.ToLower()));
                    }
                    if (model.Rating != 0)
                    {
                        managers = managers.Where(x => x.Rating.Equals(model.Rating));
                    }
                    return PartialView("List", managers.ToPagedList(1, managers.Count() == 0 ? 1 : managers.Count()));

                }
                return View();
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public JsonResult GetChartData()
        {
            var item = uow.Managers.Get().Select(x => new object[] { x.LastName, x.Order.Count }).ToArray();

            return Json(item, JsonRequestBehavior.AllowGet);
        }
        // GET: Manager/Details/5
        public ActionResult Details(int id)
        {
            return PartialView(mapper.Map<IEnumerable<OrderDTO>, IEnumerable<OrderViewModel>>(orderService.GetOrdersByManagerId(id)));
        }

        // GET: Manager/Create
        public ActionResult Create(int? page)
        {
            var model = new ManagerViewModel();
            ViewBag.CurrentPage = page;
            return View(model);
        }

        // POST: Manager/Create
        [HttpPost]
        public ActionResult Create(ManagerViewModel model, int? page)
        {
            try
            {
                // TODO: Add insert logic here
                managerService.Create(mapper.Map<ManagerViewModel, ManagerDTO>(model));
                return RedirectToAction("Index", new { page = page });
            }
            catch
            {
                return View();
            }
        }

        // GET: Manager/Edit/5
        public ActionResult Edit(int id, int? page)
        {
            ViewBag.CurrentPage = page;
            return View(mapper.Map<ManagerDTO, ManagerViewModel>(managerService.FindById(id)));
        }

        // POST: Manager/Edit/5
        [HttpPost]
        public ActionResult Edit(ManagerViewModel model, int? page)
        {
            try
            {
                managerService.Update(mapper.Map<ManagerViewModel, ManagerDTO>(model));
                return RedirectToAction("Index", new { page = page });
            }
            catch
            {
                return View();
            }
        }

        // GET: Manager/Delete/5
        public ActionResult Delete(int id, int? page)
        {
            ViewBag.CurrentPage = page;
            return View(mapper.Map<ManagerDTO, ManagerViewModel>(managerService.FindById(id)));
        }

        // POST: Manager/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection formCollection, int? page)
        {
            try
            {
                managerService.Delete(id);
                return RedirectToAction("Index", new { page = page });
            }
            catch
            {
                return View();
            }
        }
    }
}
