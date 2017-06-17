using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemMagazynowy.Models;

namespace SystemMagazynowy.DAL
{
    public interface IStocktakingRepository
    {
        void AddStocktaking(Stocktaking stocktaking);
        //{
        //    db.Stocktaking.Add(stocktaking);
        //}

        void UpdateStocktaking(Stocktaking stocktaking);

        void DeleteStocktaking(Stocktaking stocktaking);

        Stocktaking GetStocktakingByID(int? id);

        IQueryable<Stocktaking> GetAllStocktaking();

        int GetLastStocktakingID();

        int IsStocktakingOpen();

        void AddOrUpdateStocktakingAssortment(List<StocktakingAssortment> assortment);

        void UpdateStocktakingAssortment(List<StocktakingAssortment> assortment);

        void DeleteStocktakingAssortment(int? id);

        StocktakingAssortment GetStocktakingAssortmentByID(int? id);

        IQueryable<StocktakingAssortment> GetAssortmentByStocktaking(int? stocktakingID);

        IQueryable<StocktakingAssortment> GetAllStocktakingAssortment();

        void ModifyAssortmentWarehouseQuantity(int stocktakingID, int warehouseID);

    }
}
