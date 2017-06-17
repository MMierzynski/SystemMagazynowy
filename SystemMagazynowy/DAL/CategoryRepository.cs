using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemMagazynowy.Models;

namespace SystemMagazynowy.DAL
{
    public class CategoryRepository : ICategoriesRepository
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public void AddCategory(AssortmentCategory category)
        {
            db.Category.Add(category);
            db.SaveChanges();
        }

        public void DeleteCategory(AssortmentCategory category)
        {
            int assortmentCount = db.Assortment.Where(a => a.AssortmentCategoryID == category.ID).Count();

            if (assortmentCount > 0)
                return;

            db.Category.Remove(category);
            db.SaveChanges();

        }

        public IQueryable<AssortmentCategory> GetAllCategories()
        {
            return db.Category;
        }

        public AssortmentCategory GetCategoryById(int? id)
        {
            return db.Category.Find(id);
        }

        public void UpdateCategory(AssortmentCategory category)
        {
            db.Entry(category).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

        }
    }
}