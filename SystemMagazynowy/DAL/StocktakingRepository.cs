using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data;
using System.Web;
using SystemMagazynowy.Models;

namespace SystemMagazynowy.DAL
{
    public class StocktakingRepository : IStocktakingRepository
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public void AddStocktaking(Stocktaking stocktaking)
        {
            db.Stocktaking.Add(stocktaking);
            db.SaveChanges();
        }

        public Stocktaking GetStocktakingByID(int? id)
        {
            return db.Stocktaking.Find(id);
        }

        public void UpdateStocktaking(Stocktaking stocktaking)
        {
            db.Entry(stocktaking).State = EntityState.Modified;
            db.SaveChanges();
        }

        public IQueryable<Stocktaking> GetAllStocktaking()
        {
            return db.Stocktaking.Include(s => s.Assortment).Include(s => s.Warehouse).Include(s => s.User);
        }

        public int GetLastStocktakingID()
        {
            return db.Stocktaking.Select(s => s.ID).ToList().LastOrDefault();
        }

        public int IsStocktakingOpen()
        {
            var open = db.Stocktaking.Where(s => s.IsOpen == true).SingleOrDefault();

            if (open!=null)
            {
                return open.ID;
            }

            return -1;
        }

        public void AddOrUpdateStocktakingAssortment(List<StocktakingAssortment> assortment)
        {
            
            int stocktakingID = GetLastStocktakingID();
            var addedAssortment = GetAllStocktakingAssortment().Where(a => a.StocktakingID == stocktakingID).Select(a=>a.ID).ToList();

            foreach (var item in assortment)
            {
                
                item.StocktakingID = stocktakingID;
            }

            foreach(var item in assortment)
            {
                if (item.ID == 0)
                {
                    db.StacktakingAssortment.Add(item);
                }
                else
                {
                    db.Entry(item).State = EntityState.Modified;
                }


                if(addedAssortment.Contains(item.ID))
                {
                    addedAssortment.Remove(item.ID);
                }
            }
            foreach(var item in addedAssortment)
            {
                DeleteStocktakingAssortment(item);
            }
            

            db.SaveChanges();
        
           
           
        }

        public void DeleteStocktakingAssortment(int? id)
        {
            db.StacktakingAssortment.Remove(GetStocktakingAssortmentByID(id));
        }

        public IQueryable<StocktakingAssortment> GetAllStocktakingAssortment()
        {
            return db.StacktakingAssortment.OrderBy(sa=>sa.StocktakingID);
        }

        public StocktakingAssortment GetStocktakingAssortmentByID(int? id)
        {
            return db.StacktakingAssortment.SingleOrDefault(sa => sa.ID == id);
        }

        public void UpdateStocktakingAssortment(List<StocktakingAssortment> assortment)
        {
            throw new NotImplementedException();
        }

        public void ModifyAssortmentWarehouseQuantity(int stocktakingID, int warehouseID)
        {

            var assortmentList = GetAssortmentByStocktaking(stocktakingID).ToList();

            var toModify = db.AssortmentInWarehouse.Where(a => a.WarehouseID == warehouseID);


            foreach(var sItem in assortmentList)
            {
                foreach(var wItem in toModify)
                {
                    if(sItem.AssortmentID == wItem.AssortmentID)
                    {
                        wItem.Quantity = sItem.AfterQuantity;
                    }

                }
            }

            foreach (var item in toModify)
            {
                db.Entry(item).State = EntityState.Modified;
            }
            db.SaveChanges();


        }

        public IQueryable<StocktakingAssortment> GetAssortmentByStocktaking(int? stocktakingID)
        {
            return db.StacktakingAssortment.Where(a => a.StocktakingID == stocktakingID).Include(a=>a.Assortment);
        }

        private bool IsDifferentQuantity(StocktakingAssortment sAssort, AssortmentWarehouse wAssort)
        {
            if(sAssort.AfterQuantity != wAssort.Quantity)
            {
                return true;
            }

            return false;
        }

        public void DeleteStocktaking(Stocktaking stocktaking)
        {
            var assortment = GetAssortmentByStocktaking(stocktaking.ID);
            foreach(var item in assortment)
            {
                DeleteStocktakingAssortment(item.ID);
            }

            db.Stocktaking.Remove(stocktaking);
            db.SaveChanges();
        }
    }
}