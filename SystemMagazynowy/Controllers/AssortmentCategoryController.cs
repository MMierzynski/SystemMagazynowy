using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SystemMagazynowy.DAL;
using SystemMagazynowy.Models;

namespace SystemMagazynowy.Controllers
{
    public class AssortmentCategoryController : Controller
    {
        WarehouseService service = new WarehouseService();
       // private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AssortmentCategory
        public ActionResult Index()
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            return View(service.GetAllCategories().ToList());
        }

        // GET: AssortmentCategory/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssortmentCategory assortmentCategory = service.GetCategoryById(id);
            if (assortmentCategory == null)
            {
                return HttpNotFound();
            }
            return View(assortmentCategory);
        }

        // GET: AssortmentCategory/Create
        [Authorize(Roles ="canManageCategories")]
        public ActionResult Create()
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            return View();
        }

        // POST: AssortmentCategory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canManageCategories")]
        public ActionResult Create([Bind(Include = "ID,Name")] AssortmentCategory assortmentCategory)
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            if (ModelState.IsValid)
            {
                service.AddCategory(assortmentCategory);
                return RedirectToAction("Index");
            }

            return View(assortmentCategory);
        }

        // GET: AssortmentCategory/Edit/5
        [Authorize(Roles = "canManageCategories")]
        public ActionResult Edit(int? id)
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssortmentCategory assortmentCategory = service.GetCategoryById(id);
            if (assortmentCategory == null)
            {
                return HttpNotFound();
            }
            return View(assortmentCategory);
        }

        // POST: AssortmentCategory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canManageCategories")]
        public ActionResult Edit([Bind(Include = "ID,Name")] AssortmentCategory assortmentCategory)
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            if (ModelState.IsValid)
            {
                service.UpdateCategory(assortmentCategory);
                return RedirectToAction("Index");
            }
            return View(assortmentCategory);
        }

        // GET: AssortmentCategory/Delete/5
        [Authorize(Roles = "canManageCategories")]
        public ActionResult Delete(int? id)
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssortmentCategory assortmentCategory = service.GetCategoryById(id);
            if (assortmentCategory == null)
            {
                return HttpNotFound();
            }
            return View(assortmentCategory);
        }

        // POST: AssortmentCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canManageCategories")]
        public ActionResult DeleteConfirmed(int id)
        {
            AssortmentCategory assortmentCategory = service.GetCategoryById(id);
            service.DeleteCategory(assortmentCategory);
            return RedirectToAction("Index");
        }

    }
}
