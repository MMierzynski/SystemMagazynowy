using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemMagazynowy.Models;

namespace SystemMagazynowy.DAL
{
    public class OperationRepository : IOperationRepository
    {

        ApplicationDbContext db = new ApplicationDbContext();
        public void AddOperation(Operation operation)
        {
            db.Operation.Add(operation);
            db.SaveChanges();
        }

        public void DeleteOperation(Operation operation)
        {


            db.Operation.Remove(operation);
            db.SaveChanges();
        }

        public IQueryable<Operation> GetAllOperations()
        {
            return db.Operation;
        }

        public Operation GetOperationById(int? id)
        {
            return db.Operation.Find(id);
        }

        public void UpdateOperation(Operation operation)
        {
            db.Entry(operation).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }
    }
}