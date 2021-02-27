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
using Task5.Models.Filters;
using Task5.Models.Manager;
using Task5.Models.Order;
using Task5.Util;

namespace Task5.Controllers
{
    public class ManagerController : Controller
    {
        private IUnitOfWork _uow;
        private IMapper _mapper;
        private IManagerService _managerService;
        private IOrderService _orderService;

        public ManagerController()
        {
            _uow = new EFUnitOfWork();
            _mapper = new Mapper(MapperWebConfig.Configure());
            _managerService = new ManagerService(_uow);
            _orderService = new OrderService(_uow);
        }

        public ActionResult Index(int? page)
        {
            ViewBag.CurrentPage = page ?? 1;
            return View();
        }

        public ActionResult Managers(int? page)
        {
            try
            {
                var managers = _mapper.Map<IEnumerable<ManagerDTO>, IEnumerable<ManagerViewModel>>(_managerService.GetAll());
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
                    var managers = _mapper.Map<IEnumerable<ManagerDTO>, IEnumerable<ManagerViewModel>>(_managerService.GetAll());
                    if (model.LastName != null)
                    {
                        managers = managers.Where(x => x.LastName.ToLower().Contains(model.LastName.ToLower()));
                    }
                    if (model.Rating != null)
                    {
                        managers = managers.Where(x => x.Rating<=model.Rating);
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
            var item =_managerService.GetManagersWithOrdersCount();

            return Json(item, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult Details(int id)
        {
            return PartialView(_mapper.Map<IEnumerable<OrderDTO>, IEnumerable<OrderViewModel>>(_orderService.GetOrdersByManagerId(id)));
        }

        [Authorize(Roles = "admin")]
        public ActionResult Create(int? page)
        {
            var model = new ManagerViewModel();
            ViewBag.CurrentPage = page;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Create(ManagerViewModel model, int? page)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _managerService.Create(_mapper.Map<ManagerViewModel, ManagerDTO>(model));
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
            return View(_mapper.Map<ManagerDTO, ManagerViewModel>(_managerService.FindById(id)));
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(ManagerViewModel model, int? page)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _managerService.Update(_mapper.Map<ManagerViewModel, ManagerDTO>(model));
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
            return View(_mapper.Map<ManagerDTO, ManagerViewModel>(_managerService.FindById(id)));
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id, FormCollection formCollection, int? page)
        {
            try
            {
                _managerService.Delete(id);
                return RedirectToAction("Index", new { page = page });
            }
            catch
            {
                return View();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _managerService != null)
            {
                _managerService.Dispose();
                _orderService.Dispose();
                _managerService = null;
                _orderService = null;
            }
            base.Dispose(disposing);
        }
    }
}
