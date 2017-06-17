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
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using SystemMagazynowy.ViewModel;
using PagedList;

namespace SystemMagazynowy.Controllers
{
    public class OperationController : Controller
    {
        private WarehouseService service = new WarehouseService();
        //private AssortmentRepository assortmentRepository= new AssortmentRepository();
       // private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Operation
        public ActionResult Index(string sortOrder, string currrentFilter, string searchString, int? page)
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            if(searchString!= null)
            {
                page = 1;
            }
            else
            {
                searchString = currrentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            ViewBag.DateSortParam = string.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewBag.OperationSortParam = sortOrder == "operation" ? "operation_desc" : "operation";
            ViewBag.NumberSortParam = sortOrder == "number" ? "number_desc" : "number";
            ViewBag.ContractorSortParam = sortOrder == "contractor" ? "coontractor_desc" : "contractor";
            ViewBag.UserSortParam = sortOrder == "user" ? "user_desc" : "user";

            var operation =service.GetAllOperations().Include(o => o.Contractor).Include(o=>o.User);


            ViewBag.SearchValue = searchString;
            if(!string.IsNullOrEmpty(searchString))
            {
                operation = operation.Where(o => o.User.UserName.Contains(searchString)||
                                                o.Contractor.Name.Contains(searchString)||
                                                o.Type.Contains(searchString)||
                                                o.FromWarehouse.Name.Contains(searchString)||
                                                o.ToWarehouse.Name.Contains(searchString)
                                                );
                                           
                                            
                                             
            }

            switch(sortOrder)
            {
                case "date_desc":
                    {
                        operation = operation.OrderByDescending(o => o.OperationDate);
                        break;
                    }
                case "operation":
                    {
                        operation = operation.OrderBy(o => o.Type);
                        break;
                    }
                case "operation_desc":
                    {
                        operation = operation.OrderByDescending(o => o.Type);
                        break;
                    }
                case "number":
                    {
                        operation = operation.OrderBy(o => o.Number);
                        break;
                    }
                case "number_desc":
                    {
                        operation = operation.OrderByDescending(o => o.Number);
                        break;
                    }
                case "contractor":
                    {
                        operation = operation.OrderBy(o => o.Contractor.Name);
                        break;
                    }
                case "contractor_desc":
                    {
                        operation = operation.OrderByDescending(o => o.Contractor.Name);
                        break;
                    }
                case "user":
                    {
                        operation = operation.OrderBy(o => o.User.UserName);
                        break;
                    }
                case "user_desc":
                    {
                        operation = operation.OrderByDescending(o => o.User.UserName);
                        break;
                    }
                default:
                    {
                        operation = operation.OrderByDescending(o => o.ID);
                        break;
                    }
            }

            int pageSize = 25;
            int pageNumber = (page ?? 1);

            return View(operation.ToPagedList(pageNumber,pageSize));
        }


        #region AssortmentGettersForAjax
        public string GetAssortmentByContractor(int id,string barcode)

        {
            var asrtmnt = service.GetAllAssortmentFiles().Where(a=>a.ContractorID==id && a.BarCode==barcode).SingleOrDefault();

            
            if (asrtmnt != null)
            {
                AssortmentViewModel item = new AssortmentViewModel()
                {
                    ID = asrtmnt.ID,
                    Name = asrtmnt.Name,
                    BarCode = asrtmnt.BarCode,
                    Quantity = 0
                };

                string json = JsonConvert.SerializeObject(item);

                return json;
            }
            return "";
        }


        public string GetAssortmentByWarehouse(int warehouseId,string barcode)
        {
            var assortment= service.GetReplanishmentByWarehouseID(warehouseId).SingleOrDefault(r=>r.Assortment.BarCode==barcode);

            
            if(assortment!= null){
               
                AssortmentViewModel item = new AssortmentViewModel()
                {
                    ID = assortment.AssortmentID,
                    Name = assortment.Assortment.Name,
                    BarCode = assortment.Assortment.BarCode,
                    Quantity = assortment.Quantity
                };

                string json = JsonConvert.SerializeObject(item);

                return json;
            }

            return "";
        }

        public string GetAllAssortment(string barcode)
        {

            var assortment = service.GetAllAssortmentFiles().SingleOrDefault(a => a.BarCode == barcode);
            if(assortment!=null)
            {
               AssortmentViewModel item = new AssortmentViewModel()
                {
                    ID = assortment.ID,
                    Name = assortment.Name,
                    BarCode = assortment.BarCode,
                    Quantity = 0
                };

                string json = JsonConvert.SerializeObject(item);

                return json;
            }

            return "";
        }
        #endregion
        #region AddOperation
        public void AddPzOperation(PzViewModel model)
        {
            Operation operation = new Operation
            {
                Type = model.Type,
                Number = model.Number,
                OperationDate = model.OperationDate,
                ContractorID = model.ContractorID,
                UserId = model.UserID,
                ToWarehouseID = model.ToWarehouseID
            };


            service.AddOperation(operation);

        }

        public void AddPwOperation(PwViewModel model)
        {
            Operation operation = new Operation
            {
                Type = model.Type,
                Number = model.Number,
                OperationDate = model.OperationDate,
                ToWarehouseID = model.ToWarehouseID,
                UserId = model.UserID
            };

            service.AddOperation(operation);
        }

        public void AddWzOperation(WzViewModel model)
        {
            Operation operation = new Operation
            {
                Type = model.Type,
                Number = model.Number,
                OperationDate = model.OperationDate,
                FromWarehouseID = model.FromWarehouseID,
                ContractorID = model.ContractorID,
                UserId = model.UserID
            };

            service.AddOperation(operation);
        }

        public void AddMmOperation(MmViewModel model)
        {
            Operation operation = new Operation
            {
                Type = model.Type,
                Number = model.Number,
                OperationDate = model.OperationDate,
                FromWarehouseID = model.FromWarehouseID,
                ToWarehouseID = model.ToWarehouseID,
                UserId = model.UserID
            };

            service.AddOperation(operation);
        }
        public void AddRwOperation(RwViewModel model)
        {
            Operation operation = new Operation
            {
                Type = model.Type,
                Number = model.Number,
                OperationDate = model.OperationDate,
                FromWarehouseID = model.FromWarehouseID,
                UserId = model.UserID
            };

            service.AddOperation(operation);
        }

        #endregion
        #region AddGetAssortment
        

        private AssortmentWarehouse GetAssortmentWarehouseItem(int? warehouseID, int assortmentID)
        {
            //pobierz asortyment z określonego magazynu
            AssortmentWarehouse aw = service.GetReplanishmentByWarehouseID(warehouseID).SingleOrDefault(a => a.AssortmentID == assortmentID);

            //jeśli asortymetu nie ma w magazynie
            if (aw == null)
            {
                //utwórz nowy obiekt
                aw = new AssortmentWarehouse
                {
                    ID = 0,
                    AssortmentID = assortmentID,
                    WarehouseID = (int)warehouseID,
                    Quantity = 0
                };
            }

            return aw;
        }

        public void GetAssortmentFromWarehouse(AssortmentViewModel[] array, int warehouseID)
        {
            List<AssortmentWarehouse> list = GetAssortmentWarehouseList(array, warehouseID);
           
            foreach(var item in list)
            {
               foreach(AssortmentViewModel model in array)
                {
                    if (model.ID == item.AssortmentID)
                        item.Quantity -= model.Quantity;
                }


                service.UpdateReplanishment(item);
                

            }

            
        }

        private List<AssortmentWarehouse> GetAssortmentWarehouseList(AssortmentViewModel[] array, int warehouseID)
        {
            List<AssortmentWarehouse> list = new List<AssortmentWarehouse>();
            IQueryable<AssortmentWarehouse> query = service.GetReplanishmentByWarehouseID(warehouseID);

            foreach(AssortmentViewModel item in array)
            {
                AssortmentWarehouse aw = query.SingleOrDefault(a=>a.AssortmentID==item.ID);
                if (aw != null)
                    list.Add(aw);
                else
                {
                    list.Add(new AssortmentWarehouse
                    {
                        AssortmentID = item.ID,
                        WarehouseID = warehouseID,
                        Quantity = 0

                    });
                }
            }

            return list;
        }

        private List<AssortmentWarehouse> IncreaseListQuantity(AssortmentViewModel[] array,int warehouseID)
        {
            List<AssortmentWarehouse> list = GetAssortmentWarehouseList(array, warehouseID);

            foreach(AssortmentWarehouse assortment in list)
            {
                foreach(AssortmentViewModel model in array)
                {
                    if(assortment.AssortmentID == model.ID)
                    {
                        assortment.Quantity += model.Quantity;
                    }
                }
            }


            return list;

        }

        private List<AssortmentWarehouse> DecreaseListQuantity(AssortmentViewModel[] array, int warehouseID)
        {
            List<AssortmentWarehouse> list = GetAssortmentWarehouseList(array, warehouseID);

            foreach (AssortmentWarehouse assortment in list)
            {
                foreach (AssortmentViewModel model in array)
                {
                    if (assortment.AssortmentID == model.ID)
                    {
                        assortment.Quantity -= model.Quantity;
                    }
                }
            }


            return list;
        }

        private int? GetOperationWarehouseID(Operation op)
        {
            int? id = null;


            if (op.Type == "PzK" || op.Type == "PwK" || op.Type == "Pz" || op.Type == "Pw" || op.Type == "Mm" || op.Type == "MmK")
            {
                id = op.ToWarehouseID;
            }
            else if (op.Type == "WzK" || op.Type == "RwK" || op.Type == "Wz" || op.Type == "Rw")
            {
                id = op.FromWarehouseID;
            }


            return id;
        }
       


        #endregion


        public void AddOperationAssortment(List<AssortmentViewModel> array)
        {
            var operationId = service.GetAllOperations().ToList().LastOrDefault();
            var operationAssortment = new List<OperationAssortment>();
            foreach (var item in array)
            {
                operationAssortment.Add(new OperationAssortment
                {
                    AssortmentID = item.ID,
                    Quantity = item.Quantity,
                    OperationID = operationId.ID
                });
            }
            
            foreach(var item in operationAssortment)
            {
                service.AddAssortmentForOperation(item);
            }
            
        }

        private string GetDocumentTitle(string type)
        {
            switch(type)
            {
                case "Pz":
                    {
                        return "Przyjęcie zewnętrzne";
                    }
                case "Pw":
                    {
                        return "Przyjęcie wewnętrzne";
                    }
                case "Wz":
                    {
                        return "Wydanie na zewnątrz";
                    }
                case "Rw":
                    {
                        return "Rozchód wewnętrzny";
                    }
                case "Mm":
                    {
                        return "Przesunięcie międzymagazynowe";
                    }
                default: return "";
            }
        }

        public ActionResult Document(int? id)
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Operation operation = service.GetOperationById(id);
             //= db.Operation.Include(o => o.Contractor)
             //                                  .Include(o => o.FromWarehouse)
             //                                  .Include(o => o.OperationAssortment).SingleOrDefault(o => o.ID == id);

            if (operation == null)
            {
                return HttpNotFound();
            }

            ViewBag.Document = GetDocumentTitle(operation.Type);
            return View(operation);
        }

        #region Pz
        public ActionResult Pz()
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            Operation operation = new Operation();
            PzViewModel PzModel = new PzViewModel();

            var OperList = service.GetAllOperations().Where(o => o.Type == "Pz" && o.OperationDate.Year == DateTime.Today.Year).ToList();
            if (OperList.Count == 0)
                PzModel.Number = 1;
            else
                PzModel.Number = OperList.Last().Number + 1;

            PzModel.UserID = User.Identity.GetUserId();
            PzModel.Type = "Pz";
            PzModel.OperationDate = DateTime.Today;
            PzModel.Assortment = new List<AssortmentViewModel>();

            ViewBag.ContractorID = new SelectList(service.getAllContractors(), "ID", "Name");
            ViewBag.WarehouseID = new SelectList(service.GetAllWarehouses(), "ID", "Name");

            return View(PzModel);
        }

        [HttpPost, ActionName("Pz")]
        [ValidateAntiForgeryToken]
        public ActionResult PzPost(PzViewModel pzModel)
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(pzModel.SelectedAssortment))
                {
                    AddPzOperation(pzModel);

                    AssortmentViewModel[] array = JsonConvert.DeserializeObject<AssortmentViewModel[]>(pzModel.SelectedAssortment);

                    //dodanie asortymentu do listy pozycji na magazynie

                    var assortmentToWarehouse = IncreaseListQuantity(array, pzModel.ToWarehouseID);
                    service.AddAssortmentToWarehouse(assortmentToWarehouse);
                    

                    AddOperationAssortment(array.ToList());
                   
                    return RedirectToAction("Index");
                }
                else
                {
                    
                    ViewBag.ContractorID = new SelectList(service.getAllContractors(), "ID", "Name");
                    ViewBag.WarehouseID = new SelectList(service.GetAllWarehouses(), "ID", "Name");

                    return View(pzModel);
                }
            }
            
            ViewBag.ContractorID = new SelectList(service.getAllContractors(), "ID", "Name");
            ViewBag.WarehouseID = new SelectList(service.GetAllWarehouses(), "ID", "Name");

            return View(pzModel);

        }
        #endregion
        #region Pw

        public ActionResult Pw()
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            PwViewModel pwModel = new PwViewModel();

            var OperList = service.GetAllOperations().Where(o => o.Type == "Pw" && o.OperationDate.Year == DateTime.Today.Year).ToList();
            if (OperList.Count == 0)
                pwModel.Number = 1;
            else
                pwModel.Number = OperList.Last().Number + 1;

            pwModel.UserID = User.Identity.GetUserId();
            pwModel.Type = "Pw";
            pwModel.OperationDate = DateTime.Today;

            ViewBag.WarehouseID = new SelectList(service.GetAllWarehouses(), "ID", "Name");

            return View(pwModel);
        }

        [HttpPost, ActionName("Pw")]
        [ValidateAntiForgeryToken]
        public ActionResult PwPost(PwViewModel model)
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            if (ModelState.IsValid)
            {
                if(!String.IsNullOrEmpty(model.SelectedAssortment))
                {
                    AddPwOperation(model);

                    var assortmentArray = JsonConvert.DeserializeObject<AssortmentViewModel[]>(model.SelectedAssortment);
                    service.AddAssortmentToWarehouse(IncreaseListQuantity(assortmentArray,model.ToWarehouseID));
                   

                    AddOperationAssortment(assortmentArray.ToList());
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.WarehouseID = new SelectList(service.GetAllWarehouses(), "ID", "Name");
                    return View(model);
                }
            }

            ViewBag.WarehouseID = new SelectList(service.GetAllWarehouses(), "ID", "Name");
            return View(model);
        }


        #endregion
        #region Wz
        public ActionResult Wz()
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            Operation operation = new Operation();
            WzViewModel wzModel = new WzViewModel();

            var OperList = service.GetAllOperations().Where(o => o.Type == "Wz" && o.OperationDate.Year == DateTime.Today.Year).ToList();
            if (OperList.Count == 0)
                wzModel.Number = 1;
            else
                wzModel.Number = OperList.Last().Number + 1;

            wzModel.UserID = User.Identity.GetUserId();
            wzModel.Type = "Wz";
            wzModel.OperationDate = DateTime.Today;

            ViewBag.ContractorID = new SelectList(service.getAllContractors(), "ID", "Name");
            ViewBag.WarehouseID = new SelectList(service.GetAllWarehouses(), "ID", "Name");

            return View(wzModel);
        }

        [HttpPost, ActionName("Wz")]
        [ValidateAntiForgeryToken]
        public ActionResult WzPost(WzViewModel wzModel)
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            if (ModelState.IsValid)
            {
                if(!string.IsNullOrEmpty(wzModel.SelectedAssortment))
                {
                    AddWzOperation(wzModel);
                    AssortmentViewModel[] assortmentArray = JsonConvert.DeserializeObject<AssortmentViewModel[]>(wzModel.SelectedAssortment);

                    GetAssortmentFromWarehouse(assortmentArray, wzModel.FromWarehouseID);
                    
                    AddOperationAssortment(assortmentArray.ToList());


                    return RedirectToAction("Index");
                }
            }

            ViewBag.ContractorID = new SelectList(service.getAllContractors(), "ID", "Name");
            ViewBag.WarehouseID = new SelectList(service.GetAllWarehouses(), "ID", "Name");
            return View(wzModel);
        }

        #endregion
        #region Rw
        public ActionResult Rw()
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            RwViewModel model = new RwViewModel();

            var OperList = service.GetAllOperations().Where(o => o.Type == "Rw" && o.OperationDate.Year == DateTime.Today.Year).ToList();
            if (OperList.Count == 0)
                model.Number = 1;
            else
                model.Number = OperList.Last().Number + 1;

            model.UserID= User.Identity.GetUserId();
            model.Type = "Rw";
            model.OperationDate = DateTime.Today;

            ViewBag.FromWarehouseID = new SelectList(service.GetAllWarehouses(), "ID", "Name");

            return View(model);
        }


        [ValidateAntiForgeryToken]
        [HttpPost, ActionName("Rw")]
        public ActionResult RwPost(RwViewModel model)
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            if (ModelState.IsValid)
            {
                if(!string.IsNullOrEmpty(model.SelectedAssortment))
                {
                    AddRwOperation(model);
                    var array = JsonConvert.DeserializeObject<AssortmentViewModel[]>(model.SelectedAssortment);
                  //  service.AddAssortmentToWarehouse(DecreaseListQuantity(array,model.FromWarehouseID));
                    GetAssortmentFromWarehouse(array, model.FromWarehouseID);
                    AddOperationAssortment(array.ToList());
                    return RedirectToAction("Index");
                }
            }
            ViewBag.FromWarehouseID = new SelectList(service.GetAllWarehouses(), "ID", "Name");
            return View(model);
        }


        #endregion
        #region Mm
        public ActionResult Mm()
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            MmViewModel model = new MmViewModel();

            var OperList = service.GetAllOperations().Where(o => o.Type == "Mm" && o.OperationDate.Year == DateTime.Today.Year).ToList();
            if (OperList.Count == 0)
                model.Number = 1;
            else
                model.Number = OperList.Last().Number + 1;

            model.UserID = User.Identity.GetUserId();
            model.Type = "Mm";
            model.OperationDate = DateTime.Today;

            ViewBag.FromWarehouseID = new SelectList(service.GetAllWarehouses(), "ID", "Name");
            ViewBag.ToWarehouseID = new SelectList(service.GetAllWarehouses(), "ID", "Name");

            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost, ActionName("Mm")]
        public ActionResult MmPost(MmViewModel model)
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            if (ModelState.IsValid)
            {
                if(!string.IsNullOrEmpty(model.SelectedAssortment))
                {

                    AddMmOperation(model);
                    AssortmentViewModel[] array = JsonConvert.DeserializeObject<AssortmentViewModel[]>(model.SelectedAssortment);
                   
                    GetAssortmentFromWarehouse(array, model.FromWarehouseID);

                    service.AddAssortmentToWarehouse(IncreaseListQuantity(array,model.ToWarehouseID));
                    AddOperationAssortment(array.ToList());
                    return RedirectToAction("Index");

                }
            }
            
            ViewBag.FromWarehouseID = new SelectList(service.GetAllWarehouses(), "ID", "Name");
            ViewBag.ToWarehouseID = new SelectList(service.GetAllWarehouses(), "ID", "Name");

            return View(model);
        }

        #endregion




        public ActionResult Edit(int? id)
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Operation operation = service.GetOperationById(id);
            if (operation == null)
            {
                return HttpNotFound();
            }


            List<AssortmentViewModel> assortment = new List<AssortmentViewModel>();
            foreach(var item in operation.OperationAssortment)
            {
                assortment.Add(new AssortmentViewModel
                {
                    Name = item.Assortment.Name,
                    ID = item.AssortmentID,
                    BarCode = item.Assortment.BarCode,
                    Quantity = item.Quantity
                });
            }


            EditOperationViewModel model = new EditOperationViewModel
            {
                Type = operation.Type+"K",
                ContractorID = operation.ContractorID,
                FromWarehouseID = operation.FromWarehouseID,
                ToWarehouseID = operation.ToWarehouseID,
                OperationDate = DateTime.Now,
                DocumentSourceID = operation.ID,
                Assortment = assortment
               
            };
            var opList = service.GetAllOperations().Where(o => o.Type == operation.Type+"K" && o.OperationDate.Year == DateTime.Today.Year).ToList();
            if (opList.Count == 0)
                model.Number = 1;
            else
                model.Number = opList.Last().Number + 1;

            ViewBag.ContractorID = new SelectList(service.getAllContractors(), "ID", "Name");
            ViewBag.WarehouseID = new SelectList(service.GetAllWarehouses(), "ID", "Name");

            
            if (operation.Type=="Pz")
                return View("PzK",model);
            else if (operation.Type == "Wz")
                return View("WzK", model);
            else if (operation.Type == "Pw")
                return View("PwK", model);
            else if (operation.Type == "Rw")
                return View("RwK", model);
            if (operation.Type == "Mm")
                return View("MmK", model);
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: Operation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Type,Number,OperationDate,ContractorID,ToWarehouseID,FromWarehouseID,SelectedAssortment,DocumentSourceID")] EditOperationViewModel model)
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            if (ModelState.IsValid)
            {

                if (model.SelectedAssortment != null)
                {



                    Operation operation = ChangeEditModelToOperation(model);

                    if (operation == null)
                    {
                        return HttpNotFound();
                    }

                    service.AddOperation(operation);



                    ICollection<OperationAssortment> assortment = service.GetOperationById(operation.OperationID).OperationAssortment;
                    List<AssortmentViewModel> selectedAssortment = JsonConvert.DeserializeObject<AssortmentViewModel[]>(model.SelectedAssortment).ToList();
                    List<AssortmentWarehouse> mmInWarehouse = new List<AssortmentWarehouse>();
                    List<AssortmentWarehouse> inWarehouse = new List<AssortmentWarehouse>();



                    AddOperationAssortment(selectedAssortment);
                    int? mmFromWarehouseID = operation.FromWarehouseID;
                    int? warehouseID = GetOperationWarehouseID(operation);

                    //pętla po wszystkich wybramych elementach
                    foreach (var selectedItem in selectedAssortment)
                    {
                        AssortmentWarehouse aw = GetAssortmentWarehouseItem(warehouseID, selectedItem.ID);

                        foreach (var item in assortment)
                        {
                            int? itemWarehouseID = GetOperationWarehouseID(item.Operation);

                            if (warehouseID == itemWarehouseID)
                            {
                                if (selectedItem.ID == item.AssortmentID)
                                {
                                    double difference = selectedItem.Quantity - item.Quantity;
                                    if (operation.Type == "PzK" || operation.Type == "PwK")
                                    {
                                        aw.Quantity += difference;
                                    }
                                    else if (operation.Type == "WzK" || operation.Type == "RwK")
                                    {
                                        aw.Quantity -= difference;
                                    }
                                    else if(operation.Type=="MmK")
                                    {
                                        aw.Quantity += difference;
                                        AssortmentWarehouse mmAW = GetAssortmentWarehouseItem(mmFromWarehouseID, selectedItem.ID);
                                        mmAW.Quantity -= difference;
                                        mmInWarehouse.Add(mmAW);
                                    }

                                    inWarehouse.Add(aw);
                                    break;
                                }
                            }
                           
                        }

                        if (inWarehouse.Where(a => a.AssortmentID == selectedItem.ID).SingleOrDefault() == null)
                        {
                            if (operation.Type == "PzK" || operation.Type == "PwK")
                            {
                                aw.Quantity += selectedItem.Quantity;
                            }
                            else if (operation.Type == "WzK" || operation.Type == "RwK")
                            {
                                aw.Quantity -= selectedItem.Quantity;
                            }
                            else if (operation.Type == "MmK")
                            {
                                aw.Quantity += selectedItem.Quantity;
                                AssortmentWarehouse mmAW = GetAssortmentWarehouseItem(mmFromWarehouseID, selectedItem.ID);
                                mmAW.Quantity -= selectedItem.Quantity;
                                mmInWarehouse.Add(mmAW);
                            }

                            inWarehouse.Add(aw);
                        }

                    }

                    foreach (var asrtmnt in assortment)
                    {
                        int? itemWarehouseID = GetOperationWarehouseID(asrtmnt.Operation);
                        bool removeItem = false;
                        foreach (var item in inWarehouse)
                        {
                            if ((inWarehouse.SingleOrDefault(a => a.AssortmentID == asrtmnt.AssortmentID) == null)|| warehouseID!=itemWarehouseID)
                                removeItem = true;

                        }


                        if (removeItem)
                        {
                            
                            AssortmentWarehouse aw = GetAssortmentWarehouseItem(itemWarehouseID, asrtmnt.AssortmentID);


                            if (operation.Type == "PzK" || operation.Type == "PwK")
                            {
                                aw.Quantity -= asrtmnt.Quantity;
                                
                            }
                            else if (operation.Type == "WzK" || operation.Type == "RwK")
                            {
                                aw.Quantity += asrtmnt.Quantity;
                            }
                            else if(operation.Type=="MmK")
                            {
                                aw.Quantity -= asrtmnt.Quantity;
                                if (warehouseID == itemWarehouseID)
                                {
                                    AssortmentWarehouse mmAW = GetAssortmentWarehouseItem(mmFromWarehouseID, asrtmnt.AssortmentID);
                                    mmAW.Quantity += asrtmnt.Quantity;
                                    mmInWarehouse.Add(mmAW);
                                }
                            }

                            inWarehouse.Add(aw);
                        }

                    }

                    service.AddAssortmentToWarehouse(inWarehouse);
                    service.AddAssortmentToWarehouse(mmInWarehouse);


                    return RedirectToAction("Index");
                }


            }


            ViewBag.ContractorID = new SelectList(service.getAllContractors(), "ID", "Name");
            ViewBag.WarehouseID = new SelectList(service.GetAllWarehouses(), "ID", "Name");

            return View(model.Type,model);
        }

        // GET: Operation/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.IsStocktakingOn = service.IsStocktakingOpen();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Operation operation = service.GetOperationById(id);

            if (operation == null)
            {
                return HttpNotFound();
            }
            return View(operation);
        }

        // POST: Operation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Operation operation = service.GetOperationById(id);
            service.DeleteOperation(operation);
            return RedirectToAction("Index");
        }

        private Operation ChangeEditModelToOperation(EditOperationViewModel model)
        {
            //model = (EditOperationViewModel)model;
            return new Operation
            {
                Type = model.Type,
                Number = model.Number,
                OperationDate = model.OperationDate,
                ToWarehouseID = model.ToWarehouseID,
                FromWarehouseID = model.FromWarehouseID,
                ContractorID = model.ContractorID,
                OperationID = model.DocumentSourceID,
                UserId = User.Identity.GetUserId()
            };
        }
       
    }
}
