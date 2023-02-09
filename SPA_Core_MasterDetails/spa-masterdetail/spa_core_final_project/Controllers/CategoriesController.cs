using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using spa_core_final_project.Models;

namespace spa_core_final_project.Controllers
{
    public class CategoriesController : Controller
    {
        private SellManagementDBContext db;
        public CategoriesController(SellManagementDBContext db) 
        {
            this.db = db;
        }
        // GET: Categories

        public ActionResult Index()
        {
            ViewBag.products = db.Products.ToList();
            return PartialView(db.Categories.OrderByDescending(x => x.CategoryId).ToList());
        }
        public ActionResult Create()
        {
            return PartialView(new Category());
        }
        [HttpPost]
        public ActionResult Create(Category c)
        {
            if (ModelState.IsValid && (c.CategoryName != null))
            {
                db.Categories.Add(c);
                db.SaveChanges();
                return Ok("success");
            }
            return Ok("failed");
        }
        public ActionResult Edit(int id)
        {
            return PartialView(db.Categories.First(x => x.CategoryId == id));
        }
        [HttpPost]
        public ActionResult Edit(Category c)
        {
            if (ModelState.IsValid && (c.CategoryName != null))
            {
                db.Entry(c).State = EntityState.Modified;
                db.SaveChanges();
                return Ok("success");
            }
            return Ok("failed");
        }
        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                Category c = new Category() { CategoryId = (int)id };
                db.Entry(c).State = EntityState.Deleted;
                db.SaveChanges();
                return PartialView("_success");
            }
            return PartialView("_error");
        }
    }
}
