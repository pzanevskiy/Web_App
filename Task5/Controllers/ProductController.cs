using AutoMapper;
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

        public ProductController(IProductService productService)
        {
            _mapper = new Mapper(MapperWebConfig.Configure());
            _productService = productService;
        }

        public ActionResult Index(int? page)
        {
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
                    return PartialView("List", products.ToPagedList(1, products.Count() == 0 ? 1 : products.Count()));

                }
                return View();
            }
            catch
            {
                return View("Error");
            }
        }

        [Authorize]
        public ActionResult Details(int id)
        {
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
                    _productService.Create(_mapper.Map<ProductViewModel, ProductDTO>(model));
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
                    _productService.Update(_mapper.Map<ProductViewModel, ProductDTO>(model));
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
                _productService.Delete(id);
                return RedirectToAction("Index", new { page = page });
            }
            catch
            {
                return View();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _productService != null)
            {
                _productService.Dispose();               
                _productService = null;
            }
            base.Dispose(disposing);
        }
    }
}
