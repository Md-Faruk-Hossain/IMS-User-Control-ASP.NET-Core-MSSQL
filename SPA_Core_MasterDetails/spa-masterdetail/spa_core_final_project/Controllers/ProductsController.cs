using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using spa_core_final_project.Models;
using spa_core_final_project.ViewModels;
using static NuGet.Packaging.PackagingConstants;

namespace spa_core_final_project.Controllers
{
    public class ProductsController : Controller
    {
        private readonly SellManagementDBContext db;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductsController(SellManagementDBContext db, IWebHostEnvironment hostEnvironment)
        {
            this.db = db;
            _hostEnvironment = hostEnvironment;
        }

        public ActionResult Index()
        {
            ViewBag.orderDetails = db.OrderDetails.ToList();
            return PartialView(db.Products.OrderByDescending(x => x.ProductId).Include(x => x.Category).ToList());
        }
        public ActionResult Create()
        {
            ViewBag.categories = new SelectList(db.Categories, "CategoryId", "CategoryName");
            return PartialView(new ProductVM());
        }
        [HttpPost]
        public ActionResult Create(ProductVM pvm)
        {
            if (ModelState.IsValid)
            {
                Product pr = new Product()
                {
                    ProductName = pvm.ProductName,
                    CategoryId = pvm.CategoryId,
                    Price = pvm.Price,
                    SKUCode = pvm.SKUCode,
                    EntryDate = pvm.EntryDate,
                    InStock = pvm.InStock
                };

                if (pvm.Picture != null)
                {
                    string webroot = _hostEnvironment.WebRootPath;
                    string folder = "Images";
                    string filePath = Guid.NewGuid().ToString() + Path.GetExtension(Path.GetFileName(pvm.Picture.FileName));
                    string fileToSave = Path.Combine(webroot, folder, filePath);

                    using (var stream = new FileStream(fileToSave, FileMode.Create))
                    {
                        pvm.Picture.CopyToAsync(stream);
                        pr.PicturePath = "/" + folder + "/" + filePath;
                    }
                }
                else
                {
                    pr.PicturePath = "/" + "Images" + "/" + "default.jpg";
                }

                db.Products.Add(pr);
                db.SaveChanges();
                return Ok("success");
            }
            return Ok("failed");

        }
        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                Product? pr = db.Products.Find(id);

                ProductVM pvm = new ProductVM()
                {
                    ProductId = pr.ProductId,
                    ProductName = pr.ProductName,
                    CategoryId = pr.CategoryId,
                    Price = pr.Price,
                    SKUCode = pr.SKUCode,
                    EntryDate = pr.EntryDate,
                    PicturePath = pr.PicturePath,
                    InStock = pr.InStock
                };

                ViewBag.categories = new SelectList(db.Categories, "CategoryId", "CategoryName");
                return PartialView(pvm);
            }
            return View();
        }
        [HttpPost]
        public ActionResult Edit(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                Product pr = new Product()
                {
                    ProductId = productVM.ProductId,
                    ProductName = productVM.ProductName,
                    CategoryId = productVM.CategoryId,
                    Price = productVM.Price,
                    SKUCode = productVM.SKUCode,
                    EntryDate = productVM.EntryDate,
                    PicturePath = productVM.PicturePath,
                    InStock = productVM.InStock
                };

                if (productVM.Picture != null)
                {
                    string webroot = _hostEnvironment.WebRootPath;
                    string folder = "Images";
                    string filePath = Guid.NewGuid().ToString() + Path.GetExtension(Path.GetFileName(productVM.Picture.FileName));
                    string fileToSave = Path.Combine(webroot, folder, filePath);

                    using (var stream = new FileStream(fileToSave, FileMode.Create))
                    {
                        productVM.Picture.CopyToAsync(stream);
                        pr.PicturePath = "/" + folder + "/" + filePath;
                    }
                }

                db.Entry(pr).State = EntityState.Modified;
                db.SaveChanges();
                return Ok("success");
            }
            return Ok("failed");
        }

        public ActionResult Delete(int id)
        {
            var pr = db.Products.Find(id);
            if (pr != null)
            {
                db.Entry(pr).State = EntityState.Deleted;
                db.SaveChanges();
                return Ok("success");
            }
            return Ok("failed");
        }
    }
}
