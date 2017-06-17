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
    public class ContractorController : Controller
    {
        private WarehouseService service = new WarehouseService();
       

        // GET: Contractor
        public ActionResult Index(string sortOrder,string filter, string searchString, int? page)
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            if (searchString!= null)
            {
                page = 1;
            }
            else
            {
                searchString = filter;
            }


            ViewBag.Filter = searchString;


            var contractors = service.getAllContractors();

            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.AddressSortParam = sortOrder == "address" ? "address_desc" : "address";
            ViewBag.ZipSortParam = sortOrder == "zip" ? "zip_desc" : "zip";
            ViewBag.CitySortParam = sortOrder == "city" ? "city_desc" : "city";
            ViewBag.PhoneSortParam = sortOrder == "phone" ? "phone_desc" : "phone";
            ViewBag.FaxSortParam = sortOrder == "fax" ? "fax_desc" : "fax";
            ViewBag.EmailSortParam = sortOrder == "email" ? "email_desc" : "email";

            ViewBag.SearchValue = searchString;

            if(!String.IsNullOrEmpty(searchString))
            {
                contractors = contractors.Where(c => c.Name.Contains(searchString));
            }

            
            switch(sortOrder)
            {
                case "name_desc":
                    contractors = contractors.OrderByDescending(s => s.Name);
                    break;
                case "address":
                    contractors = contractors.OrderBy(s => s.Address);
                    break;
                case "address_desc":
                    contractors = contractors.OrderByDescending(s => s.Address);
                    break;
                case "zip":
                    contractors = contractors.OrderBy(s => s.ZipCode);
                    break;
                case "zip_desc":
                    contractors = contractors.OrderByDescending(s => s.ZipCode);
                    break;
                case "city":
                    contractors = contractors.OrderBy(s => s.City);
                    break;
                case "city_desc":
                    contractors = contractors.OrderByDescending(s => s.City);
                    break;
                case "phone":
                    contractors = contractors.OrderBy(s => s.PhoneNumber);
                    break;
                case "phone_desc":
                    contractors = contractors.OrderByDescending(s => s.PhoneNumber);
                    break;
                case "fax":
                    contractors = contractors.OrderBy(s => s.Fax);
                    break;
                case "fax_desc":
                    contractors = contractors.OrderByDescending(s => s.Fax);
                    break;
                case "email":
                    contractors = contractors.OrderBy(s => s.Email);
                    break;
                case "email_desc":
                    contractors = contractors.OrderByDescending(s => s.Email);
                    break;
                default:
                    contractors = contractors.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 25;
            int pageNumber = (page ?? 1);


            return View(contractors.ToPagedList(pageNumber ,pageSize));
        }

        // GET: Contractor/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contractor contractor = service.GetContractorById(id);
            if (contractor == null)
            {
                return HttpNotFound();
            }
            return View(contractor);
        }

        // GET: Contractor/Create
        [Authorize(Roles = "canManageContractors")]
        public ActionResult Create()
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            return View();
        }

        // POST: Contractor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canManageContractors")]
        public ActionResult Create([Bind(Include = "ID,Name,Address,ZipCode,City,PhoneNumber,Email")] Contractor contractor)
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            if (ModelState.IsValid)
            {
                service.AddContractor(contractor);
                return RedirectToAction("Index");
            }

            return View(contractor);
        }

        // GET: Contractor/Edit/5
        [Authorize(Roles = "canManageContractors")]
        public ActionResult Edit(int? id)
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contractor contractor = service.GetContractorById(id);
            if (contractor == null)
            {
                return HttpNotFound();
            }
            return View(contractor);
        }

        // POST: Contractor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canManageContractors")]
        public ActionResult Edit([Bind(Include = "ID,Name,Address,ZipCode,City,PhoneNumber,Email")] Contractor contractor)
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            if (ModelState.IsValid)
            {
                service.UpdateContractor(contractor);
                return RedirectToAction("Index");
            }
            return View(contractor);
        }

        // GET: Contractor/Delete/5
        [Authorize(Roles = "canManageContractors")]
        public ActionResult Delete(int? id)
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contractor contractor = service.GetContractorById(id);
            
            if (contractor == null)
            {
                return HttpNotFound();
            }

            if(contractor.Assortment.Count>0)
            {
                ViewBag.DeleteAlert = "ALERT";
            }
            return View(contractor);
        }

        // POST: Contractor/Delete/5
        [Authorize(Roles = "canManageContractors")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contractor contractor = service.GetContractorById(id);
            service.DeleteContractor(contractor);
            return RedirectToAction("Index");
        }
    }
}
