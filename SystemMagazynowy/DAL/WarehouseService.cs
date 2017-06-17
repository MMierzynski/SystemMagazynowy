using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemMagazynowy.Models;

namespace SystemMagazynowy.DAL
{
    public class WarehouseService:IStocktakingRepository,IAssortmentRepository, IWarehouseRepository,IOperationRepository,ICategoriesRepository, IContractorRepository
    {
        IStocktakingRepository _stocktaking;
        IAssortmentRepository _assortment;
        IOperationRepository _operations;
        IWarehouseRepository _warehouses;
        ICategoriesRepository _categories;
        IContractorRepository _contractors;

        public WarehouseService() 
            : this(new StocktakingRepository(), new AssortmentRepository(),new WarehouseRepository(),new OperationRepository(), new CategoryRepository(), new ContractorRepository())
        { }

        public WarehouseService(IStocktakingRepository stocktaking, IAssortmentRepository assortment,
            IWarehouseRepository warehouses, IOperationRepository operations, ICategoriesRepository categories, IContractorRepository contractors)
        {
            _stocktaking = stocktaking;
            _assortment = assortment;
            _operations = operations;
            _warehouses = warehouses;
            _categories = categories;
            _contractors = contractors;
        }

        public void AddStocktaking(Stocktaking stocktaking)
        {
            _stocktaking.AddStocktaking(stocktaking);
        }

        public void UpdateStocktaking(Stocktaking stocktaking)
        {
            _stocktaking.UpdateStocktaking(stocktaking);
        }

        public Stocktaking GetStocktakingByID(int? id)
        {
            return _stocktaking.GetStocktakingByID(id);
        }

        public IQueryable<Stocktaking> GetAllStocktaking()
        {
            return _stocktaking.GetAllStocktaking();
        }

        public int GetLastStocktakingID()
        {
            return GetLastStocktakingID();
        }

        public int IsStocktakingOpen()
        {
            return _stocktaking.IsStocktakingOpen();
        }

        public void AddOrUpdateStocktakingAssortment(List<StocktakingAssortment> assortment)
        {
            _stocktaking.AddOrUpdateStocktakingAssortment(assortment);
        }

        public void UpdateStocktakingAssortment(List<StocktakingAssortment> assortment)
        {
            _stocktaking.UpdateStocktakingAssortment(assortment);
        }

        public void DeleteStocktakingAssortment(int? id)
        {
            _stocktaking.DeleteStocktakingAssortment(id);
        }

        public StocktakingAssortment GetStocktakingAssortmentByID(int? id)
        {
            return _stocktaking.GetStocktakingAssortmentByID(id);
        }

        public IQueryable<StocktakingAssortment> GetAssortmentByStocktaking(int? stocktakingID)
        {
            return _stocktaking.GetAssortmentByStocktaking(stocktakingID);
        }

        public IQueryable<StocktakingAssortment> GetAllStocktakingAssortment()
        {
            return _stocktaking.GetAllStocktakingAssortment();
        }

        public IQueryable<AssortmentWarehouse> GetAllAssortmentInWarehouses()
        {
            return _assortment.GetAllAssortmentInWarehouses();
        }

        public Assortment GetReplanishmentByAssortmentID(int? id)
        {
            return _assortment.GetReplanishmentByAssortmentID(id);
        }

        public IQueryable<AssortmentWarehouse> GetReplanishmentByWarehouseID(int? id)
        {
            return _assortment.GetReplanishmentByWarehouseID(id);
        }

        public IQueryable<AssortmentWarehouse> GetReplanishmentByBarCode(string barcode)
        {
            return _assortment.GetReplanishmentByBarCode(barcode);
        }

        public IQueryable<Assortment> GetAllAssortmentFiles()
        {
            return _assortment.GetAllAssortmentFiles();
        }

        public Assortment GetAssortmentFileById(int? id)
        {
            return _assortment.GetAssortmentFileById(id);
        }

        public void CreateAssortmentFile(Assortment assortment)
        {
            _assortment.CreateAssortmentFile(assortment);
        }

        public void UpdateAssortmentFile(Assortment assortment)
        {
            _assortment.UpdateAssortmentFile(assortment);
        }

        public void DeleteAssortmentFile(Assortment assortment)
        {
            _assortment.DeleteAssortmentFile(assortment);
        }

        public IQueryable<Warehouse> GetAllWarehouses()
        {
            return _warehouses.GetAllWarehouses();
        }

        public Warehouse GetWarehouseById(int? id)
        {
           return _warehouses.GetWarehouseById(id);
        }

        public void AddWarehouse(Warehouse warehouse)
        {
            _warehouses.AddWarehouse(warehouse);
        }

        public void UpdateWarehouse(Warehouse warehouse)
        {
            _warehouses.UpdateWarehouse(warehouse);
        }

        public void DeleteWarehouse(Warehouse warehouse)
        {
            _warehouses.DeleteWarehouse(warehouse);
        }

        public void DeleteStocktaking(Stocktaking stocktaking)
        {
            _stocktaking.DeleteStocktaking(stocktaking);
        }

        public void ModifyAssortmentWarehouseQuantity( int stocktakingID, int warehouseID)
        {
            _stocktaking.ModifyAssortmentWarehouseQuantity( stocktakingID, warehouseID);
        }

        public void AddAssortmentToWarehouse(List<AssortmentWarehouse> assortment)
        {
            _assortment.AddAssortmentToWarehouse(assortment);
        }

        public void UpdateReplanishment(AssortmentWarehouse assortment)
        {
            _assortment.UpdateReplanishment(assortment);
        }

        public void DeleteReplanishment(AssortmentWarehouse assortment)
        {
            _assortment.DeleteReplanishment(assortment);
        }

        public IQueryable<OperationAssortment> GetAssortmentForOperation(int? operationID)
        {
            return _assortment.GetAssortmentForOperation(operationID);
        }

        public IQueryable<Operation> GetAllOperations()
        {
            return _operations.GetAllOperations();
        }

        public Operation GetOperationById(int? id)
        {
            return _operations.GetOperationById(id);
        }

        public void AddOperation(Operation operation)
        {
            _operations.AddOperation(operation);
        }

        public void UpdateOperation(Operation operation)
        {
            _operations.UpdateOperation(operation);
        }

        public void DeleteOperation(Operation operation)
        {
            _operations.DeleteOperation(operation);
        }

        public IQueryable<Contractor> getAllContractors()
        {
            return _contractors.getAllContractors();
        }

        public Contractor GetContractorById(int? id)
        {
            return _contractors.GetContractorById(id);
        }

        public void AddContractor(Contractor contractor)
        {
            _contractors.AddContractor(contractor);
        }

        public void DeleteContractor(Contractor contractor)
        {
            _contractors.DeleteContractor(contractor);
        }

        public void UpdateContractor(Contractor contractor)
        {
            _contractors.UpdateContractor(contractor);
        }

        public IQueryable<AssortmentCategory> GetAllCategories()
        {
            return _categories.GetAllCategories();
        }

        public AssortmentCategory GetCategoryById(int? id)
        {
            return _categories.GetCategoryById(id);
        }

        public void AddCategory(AssortmentCategory category)
        {
            _categories.AddCategory(category);
        }

        public void DeleteCategory(AssortmentCategory category)
        {
            _categories.DeleteCategory(category);
        }

        public void UpdateCategory(AssortmentCategory category)
        {
            _categories.UpdateCategory(category);
        }

        public void AddAssortmentForOperation(OperationAssortment assortment)
        {
            _assortment.AddAssortmentForOperation(assortment);
        }

        public void UpdateAssortmentForOperation(OperationAssortment assortment)
        {
            _assortment.UpdateAssortmentForOperation(assortment);
        }

        public void DeleteAssortmentForOperation(OperationAssortment assortment)
        {
            _assortment.DeleteAssortmentForOperation(assortment);
        }
    }
}