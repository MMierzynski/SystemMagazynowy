using System.Collections.Generic;
using System.Linq;
using SystemMagazynowy.Models;

namespace SystemMagazynowy.DAL
{
    public interface IAssortmentRepository
    {
        IQueryable<AssortmentWarehouse> GetAllAssortmentInWarehouses();

        Assortment GetReplanishmentByAssortmentID(int? id);

        IQueryable<AssortmentWarehouse> GetReplanishmentByWarehouseID(int? id);

        IQueryable<AssortmentWarehouse> GetReplanishmentByBarCode(string barcode);

        void AddAssortmentToWarehouse(List<AssortmentWarehouse> assortment);

        void UpdateReplanishment(AssortmentWarehouse assortment);

        void DeleteReplanishment(AssortmentWarehouse assortment);

        IQueryable<Assortment> GetAllAssortmentFiles();

        Assortment GetAssortmentFileById(int? id);

        void CreateAssortmentFile(Assortment assortment);

        void UpdateAssortmentFile(Assortment assortment);

        void DeleteAssortmentFile(Assortment assortment);

        IQueryable<OperationAssortment> GetAssortmentForOperation(int? operationID);

        void AddAssortmentForOperation(OperationAssortment assortment);

        void UpdateAssortmentForOperation(OperationAssortment assortment);

        void DeleteAssortmentForOperation(OperationAssortment assortment);

        
    }
}