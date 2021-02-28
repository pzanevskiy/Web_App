using AutoMapper;
using Ninject.Extensions.Logging;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Task5.BL.DTO;
using Task5.BL.Service.Interfaces;
using Task5.Models.Filters;
using Task5.Models.Product;
using Task5.Util;

namespace Task5.Controllers
{
    public class ProductController : Controller
    {
        private IMapper _mapper;
        private IProductService _productService;
        private ILogger _logger;

        public ProductController(IProductService productService, ILogger logger)
        {
            _mapper = new Mapper(MapperWebConfig.Configure());
            _productService = productService;
            _logger = logger;
        }

        public ActionResult Index(int? page)
        {
            _logger.Info($"Hello product index");
            ViewBag.CurrentPage = page ?? 1;
            return View();
        }

        public ActionResult Products(int? page)
        {
            try
            {
                var orders = _mapper.Map<IEnumerable<ProductDTO>, IEnumerable<ProductViewModel>>(_productService.GetAll());
                ViewBag.CurrentPage = page;
                return PartialView("List", orders.ToPagedList(page ?? 1, 3));
            }
            catch
            {
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Products(ProductFilter model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var products = _mapper.Map<IEnumerable<ProductDTO>, IEnumerable<ProductViewModel>>(_productService.GetAll());
                    if (model.Name != null)
                    {
                        products = products.Where(x => x.Name.ToLower().Contains(model.Name.ToLower()));
                    }
                    if (model.Price != null)
                    {
                        products = products.Where(x => x.Price.Equals(model.Price));
                    }
                    _logger.Info($"{User.Identity.Name} filter product");
                    return PartialView("List", products.ToPagedList(1, products.Count() == 0 ? 1 : products.Count()));

                }
                _logger.Warn($"{User.Identity.Name} Products filter error");
                return View();
            }
            catch
            {
                _logger.Warn($"{User.Identity.Name} Products filter error");
                return View("Error");
            }
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            _logger.Info($"{User} details product");
            return PartialView(_mapper.Map<ProductDTO, ProductViewModel>(_productService.FindById(id)));
        }

        [Authorize(Roles = "admin")]
        public ActionResult Create(int? page)
        {
            var model = new ProductViewModel();
            ViewBag.CurrentPage = page;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Create(ProductViewModel model, int? page)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.Info($"{User.Identity.Name} create product");
                    _productService.Create(_mapper.Map<ProductViewModel, ProductDTO>(model));
                    return RedirectToAction("Index", new { page = page });
                }
                _logger.Warn($"{User.Identity.Name} Products create error");
                return View();
            }
            catch
            {
                _logger.Warn($"{User.Identity.Name} Products create error");
                return View();
            }
        }

        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id,int?page)
        {
            ViewBag.CurrentPage = page;
            return View(_mapper.Map<ProductDTO, ProductViewModel>(_productService.FindById(id)));
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(ProductViewModel model, int? page)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.Info($"{User.Identity.Name} edit product");
                    _productService.Update(_mapper.Map<ProductViewModel, ProductDTO>(model));
                    return RedirectToAction("Index", new { page = page });
                }
                return View();
            }
            catch
            {
                _logger.Warn($"{User.Identity.Name} Products edit error");
                return View();
            }
        }

        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id,int? page)
        {
            ViewBag.CurrentPage = page;
            return View(_mapper.Map<ProductDTO, ProductViewModel>(_productService.FindById(id)));
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id, FormCollection collection, int? page)
        {
            try
            {
                _logger.Info($"{User.Identity.Name} delete product");
                _productService.Delete(id);
                return RedirectToAction("Index", new { page = page });
            }
            catch
            {
                _logger.Warn($"{User.Identity.Name} Products delete error");
                return View();
            }
        }        
    }
}
