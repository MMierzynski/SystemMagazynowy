using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SystemMagazynowy.Models;

namespace SystemMagazynowy.DAL
{
    public class ContractorRepository : IContractorRepository
    {

        ApplicationDbContext db = new ApplicationDbContext();

        public void AddContractor(Contractor contractor)
        {
            db.Contractor.Add(contractor);
            db.SaveChanges();
        }

        public void DeleteContractor(Contractor contractor)
        {
            var assortmentCount = db.Assortment.Where(a => a.ContractorID == contractor.ID).Count();
            if (assortmentCount > 0)
                return;
            
            db.Contractor.Remove(contractor);
            db.SaveChanges();
        }

        public IQueryable<Contractor> getAllContractors()
        {
            return db.Contractor;
        }

        public Contractor GetContractorById(int? id)
        {
            return db.Contractor.Find(id);
        }

        public void UpdateContractor(Contractor contractor)
        {
            db.Entry(contractor).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}