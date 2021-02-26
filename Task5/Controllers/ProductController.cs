using AutoMapper;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Task5.BL.DTO;
using Task5.BL.Service;
using Task5.DAL.UnitOfWork;
using Task5.DAL.UnitOfWork.Interfaces;
using Task5.Models.Filters;
using Task5.Models.Product;
using Task5.Util;

namespace Task5.Controllers
{
    public class ProductController : Controller
    {
        IUnitOfWork uow;
        IMapper mapper;
        ProductService productService;

        public ProductController()
        {
            uow = new EFUnitOfWork();
            mapper = new Mapper(MapperWebConfig.Configure());
            productService = new ProductService(uow);
        }
        // GET: Product
        public ActionResult Index(int? page)
        {
            ViewBag.CurrentPage = page ?? 1;
            return View();
        }

        public ActionResult Products(int? page)
        {
            try
            {
                var orders = mapper.Map<IEnumerable<ProductDTO>, IEnumerable<ProductViewModel>>(productService.GetAll());
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
        public ActionResult Products(ProductFilter model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var products = mapper.Map<IEnumerable<ProductDTO>, IEnumerable<ProductViewModel>>(productService.GetAll());
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
            catch (Exception e)
            {
                return View("Error");
            }
        }

        // GET: Product/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            return PartialView(mapper.Map<ProductDTO, ProductViewModel>(productService.FindById(id)));
        }

        // GET: Product/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create(int? page)
        {
            var model = new ProductViewModel();
            ViewBag.CurrentPage = page;
            return View(model);
        }

        // POST: Product/Create
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Create(ProductViewModel model, int? page)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    productService.Create(mapper.Map<ProductViewModel, ProductDTO>(model));
                    return RedirectToAction("Index", new { page = page });
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id,int?page)
        {
            ViewBag.CurrentPage = page;
            return View(mapper.Map<ProductDTO, ProductViewModel>(productService.FindById(id)));
        }

        // POST: Product/Edit/5
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(ProductViewModel model, int? page)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    productService.Update(mapper.Map<ProductViewModel, ProductDTO>(model));
                    return RedirectToAction("Index", new { page = page });
                }
                return View();
                // TODO: Add update logic here
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id,int? page)
        {
            ViewBag.CurrentPage = page;
            return View(mapper.Map<ProductDTO, ProductViewModel>(productService.FindById(id)));
        }

        // POST: Product/Delete/5
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id, FormCollection collection, int? page)
        {
            try
            {
                productService.Delete(id);
                return RedirectToAction("Index", new { page = page });
            }
            catch
            {
                return View();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && productService != null)
            {
                productService.Dispose();               
                productService = null;
            }
            base.Dispose(disposing);
        }
    }
}
