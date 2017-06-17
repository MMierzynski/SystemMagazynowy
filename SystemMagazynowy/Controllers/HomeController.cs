using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using SystemMagazynowy.DAL;
using PagedList;

namespace SystemMagazynowy.Controllers
{
    public class HomeController : Controller
    {
        private WarehouseService service = new WarehouseService();
    


        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {

            ViewBag.IsStacktakingOn = service.IsStocktakingOpen();
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var assortmentList = service.GetAllAssortmentInWarehouses();

            ViewBag.NameSortParam = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.WarehouseSortParam = sortOrder == "warehouse" ? "warehouse_desc" : "warehouse";
            ViewBag.BarCodeSortParam = sortOrder == "barcode" ? "barcode_desc" : "barcode";
            ViewBag.QuantitySortParam = sortOrder == "quantity" ? "quantity_desc" : "quantity";
            ViewBag.UnitSortParam = sortOrder == "unit" ? "unit_desc" : "unit";
            ViewBag.CatSortParam = sortOrder == "category" ? "category_desc" : "category";

            ViewBag.SearchValue = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                assortmentList = assortmentList.Where(a => a.Assortment.Name.Contains(searchString) ||
                                                            a.Assortment.BarCode.Contains(searchString));
            }


            switch (sortOrder)
            {
                case "name_desc":
                    assortmentList = assortmentList.OrderByDescending(a => a.Assortment.Name).ThenByDescending(a => a.Quantity);
                    break;
                case "warehouse":
                    assortmentList = assortmentList.OrderBy(a => a.Warehouse.ID).ThenBy(a => a.Assortment.Name);
                    break;
                case "warehouse_desc":
                    assortmentList = assortmentList.OrderByDescending(a => a.Warehouse.ID).ThenBy(a => a.Assortment.Name);
                    break;
                case "barcode":
                    assortmentList = assortmentList.OrderBy(a => a.Assortment.BarCode).ThenByDescending(a => a.Quantity);
                    break;
                case "barcode_desc":
                    assortmentList = assortmentList.OrderByDescending(a => a.Assortment.BarCode).ThenByDescending(a => a.Quantity);
                    break;
                case "quantity":
                    assortmentList = assortmentList.OrderBy(a => a.Quantity).ThenBy(a => a.Assortment.Name);
                    break;
                case "quantity_desc":
                    assortmentList = assortmentList.OrderByDescending(a => a.Quantity).ThenBy(a => a.Assortment.Name);
                    break;
                case "unit":
                    assortmentList = assortmentList.OrderBy(a => a.Assortment.Unit).ThenBy(a => a.Assortment.Name);
                    break;
                case "unit_desc":
                    assortmentList = assortmentList.OrderByDescending(a => a.Assortment.Unit).ThenBy(a => a.Assortment.Name);
                    break;
                case "category":
                    assortmentList = assortmentList.OrderBy(a => a.Assortment.Category.Name).ThenBy(a => a.Assortment.Name);
                    break;
                case "category_desc":
                    assortmentList = assortmentList.OrderByDescending(a => a.Assortment.Category.Name).ThenBy(a => a.Assortment.Name);
                    break;
                default:
                    assortmentList = assortmentList.OrderBy(a => a.Assortment.Name).ThenByDescending(a => a.Quantity);
                    break;

            }



           // FillElementsOnPageList();
            int pageSize = 25;
            //ViewBag.PageCount = pageSize;
            int pageNumber = (page ?? 1);

           // ViewBag.ElementsOnList = elementsOnPage;

            return View(assortmentList.ToPagedList(pageNumber, pageSize));
        }
    }
    
}