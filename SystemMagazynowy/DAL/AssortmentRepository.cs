using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using SystemMagazynowy.Models;

namespace SystemMagazynowy.DAL
{
    public class AssortmentRepository:IAssortmentRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void AddAssortmentForOperation(OperationAssortment assortment)
        {
            db.OperationAssortment.Add(assortment);
            db.SaveChanges();
        }

        public void AddAssortmentToWarehouse(List<AssortmentWarehouse> assortment)
        {

           
            foreach (var item in assortment)
            {
                if (item.ID==0)
                {
                    db.AssortmentInWarehouse.Add(item);
                }
                else
                {
                   db.Entry(item).State = EntityState.Modified;
                }
            }

            db.SaveChanges();
        }

        public void CreateAssortmentFile(Assortment assortment)
        {
            db.Assortment.Add(assortment);
            db.SaveChanges();
        }

        public void DeleteAssortmentFile(Assortment assortment)
        {
            db.Assortment.Remove(assortment);
            db.SaveChanges();
        }

        public void DeleteAssortmentForOperation(OperationAssortment assortment)
        {
            db.OperationAssortment.Remove(assortment);
            db.SaveChanges();
        }

        public void DeleteReplanishment(AssortmentWarehouse assortment)
        {
            db.AssortmentInWarehouse.Remove(assortment);
            db.SaveChanges();
        }

        public IQueryable<Assortment> GetAllAssortmentFiles()
        {
            return db.Assortment.Include(a => a.Category).Include(a => a.Contractor);
        }

        public IQueryable<AssortmentWarehouse> GetAllAssortmentInWarehouses()
        {
             return db.AssortmentInWarehouse.Include(a => a.Warehouse).Include(a => a.Assortment);
        }

        public Assortment GetAssortmentFileById(int? id)
        {
            return db.Assortment.Find(id); 
        }

        public IQueryable<OperationAssortment> GetAssortmentForOperation(int? operationID)
        {
            return db.OperationAssortment.Where(o => o.OperationID == operationID);
        }

        public Assortment GetReplanishmentByAssortmentID(int? id)
        {
            return db.Assortment.Where(a => a.ID == id).Include(a => a.AssortmentInWarehouse).Include(a => a.Warehouse).SingleOrDefault();
            //return db.AssortmentInWarehouse.Where(a => a.AssortmentID == id).Include(a => a.Assortment).Include(a => a.Warehouse);
        }

        public IQueryable<AssortmentWarehouse> GetReplanishmentByBarCode(string barcode)
        {
            return db.AssortmentInWarehouse.Where(a => a.Assortment.BarCode==barcode);
        }

        public IQueryable<AssortmentWarehouse> GetReplanishmentByWarehouseID(int? id)
        {
            return db.AssortmentInWarehouse.Where(a => a.WarehouseID == id).Distinct().OrderBy(a => a.Assortment.Name);
        }

        public void UpdateAssortmentFile(Assortment assortment)
        {
            db.Entry(assortment).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void UpdateAssortmentForOperation(OperationAssortment assortment)
        {
            db.Entry(assortment).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void UpdateReplanishment(AssortmentWarehouse assortment)
        {
            db.Entry(assortment).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}