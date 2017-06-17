using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SystemMagazynowy.DAL;
using SystemMagazynowy.Models;
using SystemMagazynowy.ViewModel;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using OfficeOpenXml;

namespace SystemMagazynowy.Controllers
{
    public class StocktakingsController : Controller
    {

        WarehouseService service = new WarehouseService();
        

        // GET: Stocktakings
        [Authorize(Roles = "canPerformStocktaking")]
        public ActionResult Index()
        {
            ViewBag.IsOpenStocktaking = service.IsStocktakingOpen();
            return View(service.GetAllStocktaking().ToList().OrderByDescending(s=>s.ID));
        }

        // GET: Stocktakings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Stocktaking stocktaking = service.GetStocktakingByID(id);
            var assortmentList = service.GetAssortmentByStocktaking(id);
            List<Assortment4ExcelViewModel> excelData = new List<Assortment4ExcelViewModel>();
            foreach(var item in assortmentList)
            {
                excelData.Add(Convert4Excel(item));
            }


            ExcelPackage exclPckg = new ExcelPackage();
            var sheet = exclPckg.Workbook.Worksheets.Add(stocktaking.Warehouse.Name);
            
            sheet.Cells.LoadFromCollection(excelData, true);
            sheet.Cells["A1"].Value = "Nazwa";
            sheet.Cells["B1"].Value = "Kod kreskowy";
            sheet.Cells["C1"].Value = "Jednostka";
            sheet.Cells["D1"].Value = "Ilość w systemie";
            sheet.Cells["E1"].Value = "Ilość rzeczywista";
            sheet.Cells["F1"].Value = "Różnica";
            using (var mmryStrm = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader ("content-disposition", "attachment;  filename=Inwentaryzacja_"+ stocktaking.Warehouse.Name + "_"+stocktaking.StartDate.ToShortDateString()+".xlsx");
                exclPckg.SaveAs(mmryStrm);
                mmryStrm.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
                return RedirectToAction("Index");
        }

        // GET: Stocktakings/Create
        [Authorize(Roles = "canPerformStocktaking")]
        public ActionResult Create()
        {
            OpenStacktakingViewModel model = new OpenStacktakingViewModel();
            ViewBag.IsOpenStocktaking = service.IsStocktakingOpen();

            var queryList = service.GetAllStocktaking().Where(s => s.StartDate.Year == DateTime.Now.Year).ToList();
                if (queryList.Count > 0)
                    model.Number = queryList.Count + 1;
                else
                    model.Number = 1;

                model.StartDate = DateTime.Now;
           

            model.IsOpen = true;
            model.UserID =  User.Identity.GetUserId();

            ViewBag.WarehouseID = new SelectList(service.GetAllWarehouses(), "ID", "Name");
            return View(model);
        }

        // POST: Stocktakings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canPerformStocktaking")]
        public ActionResult Create([Bind(Include = "Number,StartDate,EndDate,IsOpen,WarehouseID,UserID, AssortmentInStocktaking")] OpenStacktakingViewModel model)
        {
            ViewBag.IsOpenStocktaking = service.IsStocktakingOpen();
            if (ModelState.IsValid)
            {
                //poprawić viewmodel /////////////////////////////////////////////////////////////////////////////////////
                List<StocktakingAssortmentViewModel> assortment = new List<StocktakingAssortmentViewModel>();
                List<StocktakingAssortment> array = new List<StocktakingAssortment>();
                if (model.AssortmentInStocktaking!=null)
                    array = JsonConvert.DeserializeObject<List<StocktakingAssortment>>(model.AssortmentInStocktaking);

                Stocktaking stocktaking = new Stocktaking
                {
                    Number = model.Number,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    IsOpen = model.IsOpen,
                    WarehouseID = model.WarehouseID,
                    UserId = model.UserID
                };


                

                service.AddStocktaking(stocktaking);

                service.AddOrUpdateStocktakingAssortment(array);

                if (!stocktaking.IsOpen)
                {
                    service.ModifyAssortmentWarehouseQuantity(stocktaking.ID, stocktaking.WarehouseID);
                }

                
                return RedirectToAction("Index");
            }
            
            ViewBag.WarehouseID = new SelectList(service.GetAllWarehouses(), "ID", "Name");
            
            return View(model);
        }

        // GET: Stocktakings/Edit/5
        [Authorize(Roles = "canPerformStocktaking")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpenStacktakingViewModel model = new OpenStacktakingViewModel();

            var modelToEdit = service.GetStocktakingByID(id);

            if (modelToEdit != null)
            {
                List<StocktakingAssortmentViewModel> assortmentViewModel = new List<StocktakingAssortmentViewModel>();
                foreach (var item in modelToEdit.Assortment)
                {
                    assortmentViewModel.Add(new StocktakingAssortmentViewModel
                    {
                        ID = item.ID,
                        AssortmentID = item.AssortmentID,
                        Name = item.Assortment.Name,
                        Barcode = item.Assortment.BarCode,
                        Unit = item.Assortment.Unit,
                        BeforeQuantity = item.BeforeQuantity,
                        AfterQuantity = item.AfterQuantity
                    });
                }

                model.Number = modelToEdit.Number;
                model.StartDate = modelToEdit.StartDate;
                model.WarehouseID = modelToEdit.WarehouseID;
                model.Number = modelToEdit.Number;
                model.IsOpen = modelToEdit.IsOpen;
                model.Assortment = assortmentViewModel;
                model.UserID = User.Identity.GetUserId();
                model.AssortmentInStocktaking = JsonConvert.SerializeObject(assortmentViewModel);
            }
            else
            {
                return HttpNotFound();
            }

            
            return View(model);
        }

        // POST: Stocktakings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canPerformStocktaking")]
        public ActionResult Edit([Bind(Include = "Number,StartDate,EndDate,IsOpen,WarehouseID,UserID, AssortmentInStocktaking")] OpenStacktakingViewModel model)
        {
            if (ModelState.IsValid)
            {
                List<StocktakingAssortmentViewModel> assortment = new List<StocktakingAssortmentViewModel>();
                List<StocktakingAssortment> array = JsonConvert.DeserializeObject<List<StocktakingAssortment>>(model.AssortmentInStocktaking);
                Stocktaking stocktaking = ChangeViewModelToStocktaking(model);
                stocktaking.ID = service.GetLastStocktakingID();
                var i = 1;
                service.UpdateStocktaking(stocktaking);

                service.AddOrUpdateStocktakingAssortment(array);

                if (!stocktaking.IsOpen)
                {
                    service.ModifyAssortmentWarehouseQuantity(stocktaking.ID, stocktaking.WarehouseID);
                }

                
                return RedirectToAction("Index");

            }

            return View(model);
        }


        private Assortment4ExcelViewModel Convert4Excel(StocktakingAssortment assortment)
        {
            return new Assortment4ExcelViewModel
            {
                Name = assortment.Assortment.Name,
                Barcode = assortment.Assortment.BarCode,
                Unit = assortment.Assortment.Unit,
                BeforeQuantity = assortment.BeforeQuantity,
                AfterQuantity = assortment.AfterQuantity,
                Difference = assortment.AfterQuantity- assortment.BeforeQuantity
            };
        }

        private Stocktaking ChangeViewModelToStocktaking(OpenStacktakingViewModel viewModel)
        {
            return new Stocktaking
            {
                Number = viewModel.Number,
                StartDate = viewModel.StartDate,
                EndDate = viewModel.EndDate,
                IsOpen = viewModel.IsOpen,
                WarehouseID = viewModel.WarehouseID,
                UserId = viewModel.UserID
            };
        }
        public string GetAssortmentFromWarehouseByCode(int? id,string barcode)
        {
           // int? i = id;
           // string b = barcode;
            var queryList = service.GetReplanishmentByBarCode(barcode).Where(a=>a.WarehouseID == id).SingleOrDefault();

            if (queryList != null)
            {
                StocktakingAssortmentViewModel assortmentList = new StocktakingAssortmentViewModel
                {

                    AssortmentID = queryList.AssortmentID,
                    Name = queryList.Assortment.Name,
                    Barcode = queryList.Assortment.BarCode,
                    BeforeQuantity = queryList.Quantity,
                    AfterQuantity = 0,
                    WarehouseName = queryList.Warehouse.Name,
                    Unit = queryList.Assortment.Unit
                };

                return JsonConvert.SerializeObject(assortmentList);
            }
            else
                return "";
        }

        // GET: Stocktakings/Delete/5
        [Authorize(Roles = "canPerformStocktaking")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stocktaking stocktaking = service.GetStocktakingByID(id);
            ViewBag.IsOpen = stocktaking.IsOpen;
            if (stocktaking == null)
            {
                return HttpNotFound();
            }
            return View(stocktaking);
        }

        // POST: Stocktakings/Delete/5
        [Authorize(Roles = "canPerformStocktaking")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Stocktaking stocktaking = service.GetStocktakingByID(id);
            service.DeleteStocktaking(stocktaking);
            return RedirectToAction("Index");
        }

        
    }
}
