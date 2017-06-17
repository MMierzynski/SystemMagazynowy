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
using SystemMagazynowy.ViewModel;

namespace SystemMagazynowy.Controllers
{

    public class AssortmentController : Controller
    {
        private WarehouseService service = new WarehouseService();
        //ApplicationDbContext db = new ApplicationDbContext();

        // GET: Assortment
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {

            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            if(searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var assortment = service.GetAllAssortmentFiles();

            ViewBag.NameSortParam = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.ContractorSortParam = sortOrder == "contractor" ? "contractor_desc" : "contractor";
            ViewBag.BarCodeSortParam = sortOrder == "barcode" ? "barcode_desc" : "barcode";
            ViewBag.CategorySortParam = sortOrder == "category" ? "category_desc" : "category";


            if(searchString!= null)
            {
                assortment = assortment.Where(a => a.Name.Contains(searchString) || a.BarCode.Contains(searchString));
            }

            switch(sortOrder)
            {
                case "name_desc":
                    assortment = assortment.OrderByDescending(a => a.Name).ThenByDescending(a => a.Category.Name);
                    break;
                case "barcode":
                    assortment = assortment.OrderBy(a => a.BarCode).ThenByDescending(a => a.Category.Name);
                    break;
                case "barcode_desc":
                    assortment = assortment.OrderByDescending(a => a.BarCode).ThenByDescending(a => a.Category.Name);
                    break;
                case "contractor":
                    assortment = assortment.OrderBy(a => a.Contractor.Name).ThenByDescending(a => a.Category.Name);
                    break;
                case "contractor_desc":
                    assortment = assortment.OrderByDescending(a => a.Contractor.Name).ThenByDescending(a => a.Category.Name);
                    break;
                case "category":
                    assortment = assortment.OrderBy(a => a.Category.Name).ThenBy(a => a.Name);
                    break;
                case "category_desc":
                    assortment = assortment.OrderByDescending(a => a.Category.Name).ThenBy(a => a.Name);
                    break;
                default:
                    assortment = assortment.OrderBy(a => a.Name).ThenByDescending(a => a.Category.Name);
                    break;
            }

            int pageSize = 25;
            int pageNumber = (page ?? 1);

            return View(assortment.ToPagedList(pageNumber, pageSize));
        }

        // GET: Assortment/Details/5
        public ActionResult Details(int? id, bool? showOperations)
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var query = service.GetReplanishmentByAssortmentID(id);

            AssortmentDetailsViewModel details = new AssortmentDetailsViewModel();
            if (query != null)
            {
                details.ID = query.ID;
                details.Name = query.Name;
                details.BarCode = query.BarCode;
                details.ContractorID = query.ContractorID;
                details.ContractorName = query.Contractor.Name;
                details.Unit = query.Unit;
                details.MaxQuantity = query.MaximumQuantity;
                details.MinQuantity = query.MinimumQuantity;
                details.CategoryName = query.Category.Name;

                details.Replanishment = new List<ShortAssortmentInWarehouse>();
                foreach(var item in query.AssortmentInWarehouse)
                {
                    details.Replanishment.Add(new ShortAssortmentInWarehouse()
                    {
                        WarehouseName = item.Warehouse.Name,
                        Quantity = item.Quantity
                    });
                }

                if (showOperations == true)
                {
                    try
                    {
                        details.Operations = new List<ShortOperationViewModel>();
                        var operations = query.OperationOnAssortment.Where(o => o.AssortmentID == id);//db.OperationAssortment.Include(o => o.Operation).Where(o => o.AssortmentID == id);
                        foreach(var item in operations)
                        {
                            details.Operations.Add(new ShortOperationViewModel {
                                Type = item.Operation.Type,
                                FullNumber = item.Operation.FullNumber,
                                Quantity = item.Quantity,
                                OperationDate = item.Operation.OperationDate,
                                User = item.Operation.User.UserName
                                
                            });
                        }
                        
                       // var a = 0;
                    }
                    catch(ArgumentNullException)
                    {
                        details.Operations = null;
                    }
                }

            }
            //Assortment assortment = assortmentRepository.GetAssortmentFileById(id);
            if (details == null)
            {
                return HttpNotFound();
            }
            return View(details);
        }

        // GET: Assortment/Create
        [Authorize(Roles ="canManageAssortment")]
        public ActionResult Create()
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            ViewBag.AssortmentCategoryID = new SelectList(service.GetAllCategories(), "ID", "Name");
            ViewBag.ContractorID = new SelectList(service.getAllContractors(), "ID", "Name");
            return View();
        }

        // POST: Assortment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canManageAssortment")]
        public ActionResult Create([Bind(Include = "ID,Name,BarCode,Unit,MinimumQuantity,MaximumQuantity,InitialQuantity,ContractorID,AssortmentCategoryID")] Assortment assortment)
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            if (ModelState.IsValid)
            {
                service.CreateAssortmentFile(assortment);
                return RedirectToAction("Index");
            }

            ViewBag.AssortmentCategoryID = new SelectList(service.GetAllCategories(), "ID", "Name", assortment.AssortmentCategoryID);
            ViewBag.ContractorID = new SelectList(service.getAllContractors(), "ID", "Name", assortment.ContractorID);
            return View(assortment);
        }

        // GET: Assortment/Edit/5
        [Authorize(Roles = "canManageAssortment")]
        public ActionResult Edit(int? id)
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assortment assortment = service.GetAssortmentFileById(id);
            if (assortment == null)
            {
                return HttpNotFound();
            }
            ViewBag.AssortmentCategoryID = new SelectList(service.GetAllCategories(), "ID", "Name", assortment.AssortmentCategoryID);
            ViewBag.ContractorID = new SelectList(service.getAllContractors(), "ID", "Name", assortment.ContractorID);
            return View(assortment);
        }

        // POST: Assortment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canManageAssortment")]
        public ActionResult Edit([Bind(Include = "ID,Name,BarCode,Unit,MinimumQuantity,MaximumQuantity,InitialQuantity,ContractorID,AssortmentCategoryID")] Assortment assortment)
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            if (ModelState.IsValid)
            {
                service.UpdateAssortmentFile(assortment);
                return RedirectToAction("Index");
            }
            ViewBag.AssortmentCategoryID = new SelectList(service.GetAllCategories(), "ID", "Name", assortment.AssortmentCategoryID);
            ViewBag.ContractorID = new SelectList(service.getAllContractors(), "ID", "Name", assortment.ContractorID);
            return View(assortment);
        }

        // GET: Assortment/Delete/5
        [Authorize(Roles = "canManageAssortment")]
        public ActionResult Delete(int? id)
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assortment assortment = service.GetAssortmentFileById(id);
            if(assortment.AssortmentInWarehouse.Count>0)
            {
                ViewBag.DeleteAlert = "ALERT";
            }

            if (assortment == null)
            {
                return HttpNotFound();
            }
            return View(assortment);
        }

        // POST: Assortment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canManageAssortment")]
        public ActionResult DeleteConfirmed(int id)
        {
            Assortment assortment = service.GetAssortmentFileById(id);
            service.DeleteAssortmentFile(assortment);
            return RedirectToAction("Index");
        }

        
    }
}
