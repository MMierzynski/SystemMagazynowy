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
using PagedList;

namespace SystemMagazynowy.Controllers
{
    public class WarehouseController : Controller
    {
        //private ApplicationDbContext db = new ApplicationDbContext();
        private WarehouseService service = new WarehouseService();

        // GET: Warehouse

        public ActionResult Index()
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();

            return View(service.GetAllWarehouses().ToList());
        }

        // GET: Warehouse/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Warehouse warehouse = service.GetWarehouseById(id);

            ViewBag.AllCount = service.GetAllOperations().Where(o => o.ToWarehouseID == id || o.FromWarehouseID == id).Count();
            ViewBag.PzAllCount = service.GetAllOperations().Where(o => o.Type.Contains("Pz") && (o.ToWarehouseID==id || o.FromWarehouseID==id)).Count();
            ViewBag.PwAllCount = service.GetAllOperations().Where(o => o.Type.Contains("Pw") && (o.ToWarehouseID == id || o.FromWarehouseID == id)).Count();
            ViewBag.WzAllCount = service.GetAllOperations().Where(o => o.Type.Contains("Wz") && (o.ToWarehouseID == id || o.FromWarehouseID == id)).Count();
            ViewBag.RwAllCount = service.GetAllOperations().Where(o => o.Type.Contains("Rw") && (o.ToWarehouseID == id || o.FromWarehouseID == id)).Count();
            ViewBag.MmAllCount = service.GetAllOperations().Where(o => o.Type.Contains("Mm") && (o.ToWarehouseID == id || o.FromWarehouseID == id)).Count();


            ViewBag.MonthCount = service.GetAllOperations().Where(o => (o.ToWarehouseID == id || o.FromWarehouseID == id) && o.OperationDate.Month==DateTime.Now.Month).Count();
            ViewBag.PzMonthCount = service.GetAllOperations().Where(o => o.Type.Contains("Pz") && (o.ToWarehouseID == id || o.FromWarehouseID == id) && o.OperationDate.Month == DateTime.Now.Month).Count();
            ViewBag.PwMonthCount = service.GetAllOperations().Where(o => o.Type.Contains("Pw") && (o.ToWarehouseID == id || o.FromWarehouseID == id) && o.OperationDate.Month == DateTime.Now.Month).Count();
            ViewBag.WzMonthCount = service.GetAllOperations().Where(o => o.Type.Contains("Wz") && (o.ToWarehouseID == id || o.FromWarehouseID == id) && o.OperationDate.Month == DateTime.Now.Month).Count();
            ViewBag.RwMonthCount = service.GetAllOperations().Where(o => o.Type.Contains("Rw") && (o.ToWarehouseID == id || o.FromWarehouseID == id) && o.OperationDate.Month == DateTime.Now.Month).Count();
            ViewBag.MmMonthCount = service.GetAllOperations().Where(o => o.Type.Contains("Mm") && (o.ToWarehouseID == id || o.FromWarehouseID == id) && o.OperationDate.Month == DateTime.Now.Month).Count();

            var week = DateTime.Now.AddDays(-7).Date;
            ViewBag.WeekCount = service.GetAllOperations().Where(o => (o.ToWarehouseID == id || o.FromWarehouseID == id) && o.OperationDate  >= week).Count();
            ViewBag.PzWeekCount = service.GetAllOperations().Where(o => o.Type.Contains("Pz") && (o.ToWarehouseID == id || o.FromWarehouseID == id) && o.OperationDate >= week).Count();
            ViewBag.PwWeekCount = service.GetAllOperations().Where(o => o.Type.Contains("Pw") && (o.ToWarehouseID == id || o.FromWarehouseID == id) && o.OperationDate >= week).Count();
            ViewBag.WzWeekCount = service.GetAllOperations().Where(o => o.Type.Contains("Wz") && (o.ToWarehouseID == id || o.FromWarehouseID == id) && o.OperationDate >= week).Count();
            ViewBag.RwWeekCount = service.GetAllOperations().Where(o => o.Type.Contains("Rw") && (o.ToWarehouseID == id || o.FromWarehouseID == id) && o.OperationDate >= week).Count();
            ViewBag.MmWeekCount = service.GetAllOperations().Where(o => o.Type.Contains("Mm") && (o.ToWarehouseID == id || o.FromWarehouseID == id) && o.OperationDate >= week).Count();


            ViewBag.TodayCount = service.GetAllOperations().Where(o => (o.ToWarehouseID == id || o.FromWarehouseID == id) && o.OperationDate == DateTime.Today).Count();
            ViewBag.PzTodayCount = service.GetAllOperations().Where(o => o.Type.Contains("Pz") && (o.ToWarehouseID == id || o.FromWarehouseID == id) && o.OperationDate == DateTime.Today).Count();
            ViewBag.PwTodayCount = service.GetAllOperations().Where(o => o.Type.Contains("Pw") && (o.ToWarehouseID == id || o.FromWarehouseID == id) && o.OperationDate == DateTime.Today).Count();
            ViewBag.WzTodayCount = service.GetAllOperations().Where(o => o.Type.Contains("Wz") && (o.ToWarehouseID == id || o.FromWarehouseID == id) && o.OperationDate == DateTime.Today).Count();
            ViewBag.RwTodayCount = service.GetAllOperations().Where(o => o.Type.Contains("Rw") && (o.ToWarehouseID == id || o.FromWarehouseID == id) && o.OperationDate == DateTime.Today).Count();
            ViewBag.MmTodayCount = service.GetAllOperations().Where(o => o.Type.Contains("Mm") && (o.ToWarehouseID == id || o.FromWarehouseID == id) && o.OperationDate == DateTime.Today).Count();


            if (warehouse == null)
            {
                return HttpNotFound();
            }
            return View(warehouse);
        }

        // GET: Warehouse/Create
        [Authorize(Roles = "canManageWarehouses")]
        public ActionResult Create()
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            return View();
        }

        // POST: Warehouse/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canManageWarehouses")]
        public ActionResult Create([Bind(Include = "ID,Name")] Warehouse warehouse)
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            if (ModelState.IsValid)
            {
                service.AddWarehouse(warehouse);
                return RedirectToAction("Index");
            }

            return View(warehouse);
        }

        // GET: Warehouse/Edit/5
        [Authorize(Roles = "canManageWarehouses")]
        public ActionResult Edit(int? id)
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Warehouse warehouse = service.GetWarehouseById(id);
            if (warehouse == null)
            {
                return HttpNotFound();
            }
            return View(warehouse);
        }

        // POST: Warehouse/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canManageWarehouses")]
        public ActionResult Edit([Bind(Include = "ID,Name")] Warehouse warehouse)
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            if (ModelState.IsValid)
            {
                service.UpdateWarehouse(warehouse);

                return RedirectToAction("Index");
            }
            return View(warehouse);
        }

        // GET: Warehouse/Delete/5
        [Authorize(Roles = "canManageWarehouses")]
        public ActionResult Delete(int? id)
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Warehouse warehouse = service.GetWarehouseById(id);
            ViewBag.AssortmentInWarehouse = service.GetReplanishmentByWarehouseID(id).ToList();

            if (warehouse == null)
            {
                return HttpNotFound();
            }
            return View(warehouse);
        }

        // POST: Warehouse/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canManageWarehouses")]
        public ActionResult DeleteConfirmed(int id)
        {
            Warehouse warehouse = service.GetWarehouseById(id);
            service.DeleteWarehouse(warehouse);
            return RedirectToAction("Index");
        }

    }
}
