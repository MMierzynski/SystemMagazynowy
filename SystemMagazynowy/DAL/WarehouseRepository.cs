using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemMagazynowy.Models;
using System.Data.Entity;

namespace SystemMagazynowy.DAL
{
    public class WarehouseRepository : IWarehouseRepository
    {
        ApplicationDbContext db = new ApplicationDbContext();


        public void AddWarehouse(Warehouse warehouse)
        {
            db.Warehouse.Add(warehouse);
            db.SaveChanges();
        }

        public void DeleteWarehouse(Warehouse warehouse)
        {
            db.Warehouse.Remove(warehouse);
            db.SaveChanges();
        }

        public IQueryable<Warehouse> GetAllWarehouses()
        {
            return db.Warehouse;
        }

        public Warehouse GetWarehouseById(int? id)
        {
            return db.Warehouse.Find(id);
        }

        public void UpdateWarehouse(Warehouse warehouse)
        {
            db.Entry(warehouse).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}