using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SystemMagazynowy.Models;

namespace SystemMagazynowy.DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Assortment> Assortment { get; set; }
        public DbSet<AssortmentCategory> Category { get; set; }
        public DbSet<Warehouse> Warehouse { get; set; }
        public DbSet<Contractor> Contractor { get; set; }
        public DbSet<AssortmentWarehouse> AssortmentInWarehouse { get; set; }
        public DbSet<Operation> Operation { get; set; }
        public DbSet<OperationAssortment> OperationAssortment { get; set; }
        public DbSet<Stocktaking> Stocktaking { get; set; }
        public DbSet<StocktakingAssortment> StacktakingAssortment { get; set; }

    }
}