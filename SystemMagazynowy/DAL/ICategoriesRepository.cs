using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemMagazynowy.Models;

namespace SystemMagazynowy.DAL
{
    public interface ICategoriesRepository
    {
        IQueryable<AssortmentCategory> GetAllCategories();

        AssortmentCategory GetCategoryById(int? id);

        void AddCategory(AssortmentCategory category);

        void DeleteCategory(AssortmentCategory category);

        void UpdateCategory(AssortmentCategory category);
    }
}
