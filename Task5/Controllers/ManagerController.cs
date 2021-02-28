using AutoMapper;
using Ninject.Extensions.Logging;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Task5.BL.DTO;
using Task5.BL.Service.Interfaces;
using Task5.Models.Filters;
using Task5.Models.Manager;
using Task5.Models.Order;
using Task5.Util;

namespace Task5.Controllers
{
    public class ManagerController : Controller
    {
        private IMapper _mapper;
        private IManagerService _managerService;
        private IOrderService _orderService;
        private ILogger _logger;

        public ManagerController(IManagerService managerService, IOrderService orderService, ILogger logger)
        {
            _mapper = new Mapper(MapperWebConfig.Configure());
            _managerService = managerService;
            _orderService = orderService;
            _logger = logger;
        }

        public ActionResult Index(int? page)
        {
            _logger.Info("Hello manager index");
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
            catch
            {
                _logger.Warn($"{User} Managers error");
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
                    _logger.Info($"{User.Identity.Name} filter manager");
                    return PartialView("List", managers.ToPagedList(1, managers.Count() == 0 ? 1 : managers.Count()));

                }
                _logger.Warn($"{User.Identity.Name} Managers filter error");
                return View();
            }
            catch
            {
                _logger.Warn($"{User.Identity.Name} Managers filter error");
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
            _logger.Info($"{User.Identity.Name} details manager");
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
                    _logger.Info($"{User.Identity.Name} create manager");
                    _managerService.Create(_mapper.Map<ManagerViewModel, ManagerDTO>(model));
                    return RedirectToAction("Index", new { page = page });
                }
                _logger.Warn($"{User.Identity.Name} Managers create error");
                return View();
            }
            catch
            {
                _logger.Warn($"{User.Identity.Name} Managers create error");
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
                    _logger.Info($"{User.Identity.Name} edit manager");
                    _managerService.Update(_mapper.Map<ManagerViewModel, ManagerDTO>(model));
                    return RedirectToAction("Index", new { page = page });
                }
                _logger.Warn($"{User.Identity.Name} Managers edit error");
                return View();
            }
            catch
            {
                _logger.Warn($"{User.Identity.Name} Managers edit error");
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
                _logger.Info($"{User.Identity.Name} delete manager");
                _managerService.Delete(id);
                return RedirectToAction("Index", new { page = page });
            }
            catch
            {
                _logger.Warn($"{User.Identity.Name} Managers delete error");
                return View();
            }
        }
    }
}
