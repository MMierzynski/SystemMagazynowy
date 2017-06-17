using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemMagazynowy.Models;

namespace SystemMagazynowy.DAL
{
    public interface IWarehouseRepository
    {
        IQueryable<Warehouse> GetAllWarehouses();
        Warehouse GetWarehouseById(int? id);
        void AddWarehouse(Warehouse warehouse);
        void UpdateWarehouse(Warehouse warehouse);
        void DeleteWarehouse(Warehouse warehouse);
    }
}
