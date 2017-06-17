namespace SystemMagazynowy.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using SystemMagazynowy.DAL;
    using SystemMagazynowy.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<SystemMagazynowy.DAL.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        void CreateRoles(string[] roles, ApplicationDbContext context)
        {
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            foreach(string role in roles)
            {
                rm.Create(new IdentityRole(role));
            }
        }

        bool CreateUser(ApplicationUser user,string password, string[] roles,  ApplicationDbContext context)
        {
            IdentityResult result;
            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            result = um.Create(user, password);
            if (result.Succeeded == false)
                return result.Succeeded;
            result = um.AddToRoles(user.Id, roles);
            return result.Succeeded;
        }

        void AddUserAndRole(ApplicationDbContext context)
        {
            string[] roles = { "canManageUsers",
                                 "canManageContractors",
                                 "canManageCategories",
                                 "canManageWarehouses",
                                 "canManageAssortment",
                                 "canPerformStocktaking",
                                 "canOperateWithAssortment" };

            CreateRoles(roles, context);
            
            var userAdmin = new ApplicationUser
            {
                UserName = "JanKowalski",
                PhoneNumber = "753897654",
                Email="jan.kowalski@gmail.com"
            };

            var userKierownik = new ApplicationUser
            {
                UserName = "TomaszJankowski",
                PhoneNumber = "798654125",
                Email = "jankowski-t@gmail.com"
            };

            var userMagazynier = new ApplicationUser
            {
                UserName = "AdamNowacki",
                PhoneNumber = "665259784",
                Email = "adamnowacki85@gmail.com"
            };

            CreateUser(userAdmin, "Pass_word4Admin", roles, context);
            CreateUser(userKierownik, "Pass_w0rd", new string []{ 
                                 "canManageContractors",
                                 "canManageCategories",
                                 "canManageAssortment",
                                 "canPerformStocktaking",
                                 "canOperateWithAssortment" }, context);
            CreateUser(userMagazynier, "A2N_Pass", new string[]{"canOperateWithAssortment" }, context);
        }

        protected override void Seed(SystemMagazynowy.DAL.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            AddUserAndRole(context);

            //wype³nienie bazy przyk³adowymi danymi
            //
            // lista magazynów
            var warehouses = new List<Warehouse>()
            {
                new Warehouse{Name = "MAG_1"},
                new Warehouse{Name="MAG_ZW"}
            };

            warehouses.ForEach(w => context.Warehouse.AddOrUpdate(a => a.Name, w));
            context.SaveChanges();

            //kategorie asortymentu
            var categories = new List<AssortmentCategory>()
            {
                new AssortmentCategory{Name="Towar"},
                new AssortmentCategory{Name = "Materia³"},
                new AssortmentCategory{Name="Wyrób gotowy"}
            };

            categories.ForEach(cat => context.Category.AddOrUpdate(c => c.Name, cat));
            context.SaveChanges();

            //kontrahenci
            var contractors = new List<Contractor>() 
            {
                new Contractor{Name="Top Software", Address="ul. Mickiewicza 116", ZipCode="42-200", City="Czêstochowa", PhoneNumber=348457898, NIP="9821234658", REGON="123456789", Email="zamowienia@top-software.pl"},
                new Contractor{Name="Komputronik", Address="ul. Nowa 1a", ZipCode="42-200", City="Czêstochowa", PhoneNumber=348457898, NIP="9876543210", REGON="1472589638", Email="kontakt@komputronik.pl"},
                new Contractor{Name="MyPC", Address="al. Jana Paw³a 2 18 ", ZipCode="42-200", City="Czêstochowa", PhoneNumber=348457898, NIP="6543251879", REGON="95162384751597", Email="zamowienia_rtv_hurt@gmail.com"}
            };

            contractors.ForEach(c => context.Contractor.AddOrUpdate(a=>a.NIP, c));
            context.SaveChanges();

            //asortyment
            var assortment = new List<Assortment>()
            {
                new Assortment{
                    Name="WiedŸmin 3: Dziki Gon PS4",
                    AssortmentCategoryID=categories.Single(c=>c.Name=="Towar").ID,
                    BarCode="5907610748924",
                    Unit="szt.",
                    MinimumQuantity=5,
                    MaximumQuantity=200,
                    InitialQuantity =0,
                    ContractorID = contractors.Single(c=>c.Name=="Top Software").ID
                },
                new Assortment{
                    Name=" Assassins Creed Rogue PC",
                    AssortmentCategoryID=categories.Single(c=>c.Name=="Towar").ID,
                    BarCode="3307215801468",
                    Unit="szt.",
                    MinimumQuantity=5,
                    MaximumQuantity=200,
                    InitialQuantity =0,
                    ContractorID = contractors.Single(c=>c.Name=="Top Software").ID
                },

                new Assortment{
                    Name="Battlefield 4 PS3",
                    AssortmentCategoryID=categories.Single(c=>c.Name=="Towar").ID,
                    BarCode="5030943112190",
                    Unit="szt.",
                    MinimumQuantity=5,
                    MaximumQuantity=200,
                    InitialQuantity =0,
                    ContractorID = contractors.Single(c=>c.Name=="Top Software").ID
                },

                new Assortment{
                    Name="Dragon Age: Inkwizycja Deluxe PS4",
                    AssortmentCategoryID=categories.Single(c=>c.Name=="Towar").ID,
                    BarCode="5035228113794",
                    Unit="szt.",
                    MinimumQuantity=5,
                    MaximumQuantity=200,
                    InitialQuantity =0,
                    ContractorID = contractors.Single(c=>c.Name=="Top Software").ID
                },

                new Assortment{
                    Name="Dragon Ball Z: Battle of Z PS3",
                    AssortmentCategoryID=categories.Single(c=>c.Name=="Towar").ID,
                    BarCode="3391891976749",
                    Unit="szt.",
                    MinimumQuantity=5,
                    MaximumQuantity=200,
                    InitialQuantity =0,
                    ContractorID = contractors.Single(c=>c.Name=="Top Software").ID
                },

                new Assortment{
                    Name="Driveclub PS4",
                    AssortmentCategoryID=categories.Single(c=>c.Name=="Towar").ID,
                    BarCode="7117192775760",
                    Unit="szt.",
                    MinimumQuantity=5,
                    MaximumQuantity=200,
                    InitialQuantity =0,
                    ContractorID = contractors.Single(c=>c.Name=="Top Software").ID
                },

                new Assortment{
                    Name="Ethan: £owca meteorytów PC",
                    AssortmentCategoryID=categories.Single(c=>c.Name=="Towar").ID,
                    BarCode="5906660760290",
                    Unit="szt.",
                    MinimumQuantity=5,
                    MaximumQuantity=200,
                    InitialQuantity =0,
                    ContractorID = contractors.Single(c=>c.Name=="Top Software").ID
                },

                new Assortment{
                    Name="Envolve PS4",
                    AssortmentCategoryID=categories.Single(c=>c.Name=="Towar").ID,
                    BarCode="5026555417228",
                    Unit="szt.",
                    MinimumQuantity=5,
                    MaximumQuantity=200,
                    InitialQuantity =0,
                    ContractorID = contractors.Single(c=>c.Name=="Top Software").ID
                },

                new Assortment{
                    Name="FIFA 15 PC",
                    AssortmentCategoryID=categories.Single(c=>c.Name=="Towar").ID,
                    BarCode="5030944113226",
                    Unit="szt.",
                    MinimumQuantity=5,
                    MaximumQuantity=200,
                    InitialQuantity =0,
                    ContractorID = contractors.Single(c=>c.Name=="Top Software").ID
                },

                new Assortment{
                    Name="FIFA 16 PS4",
                    AssortmentCategoryID=categories.Single(c=>c.Name=="Towar").ID,
                    BarCode="5035226112874",
                    Unit="szt.",
                    MinimumQuantity=5,
                    MaximumQuantity=200,
                    InitialQuantity =0,
                    ContractorID = contractors.Single(c=>c.Name=="Top Software").ID
                }
            };

            assortment.ForEach(a => context.Assortment.AddOrUpdate(b=>b.Name,a));
            context.SaveChanges();

            //var assortmentInWarehouse = new List<AssortmentWarehouse>()
            //{
            //    new AssortmentWarehouse
            //    {
            //        WarehouseID=warehouses.Single(w=>w.Name=="MAG_1").ID,
            //        AssortmentID = assortment.Single(a=>a.Name=="Envolve PS4").ID,
            //        Quantity = 50.0
            //    },
            //    new AssortmentWarehouse
            //    {
            //        WarehouseID=warehouses.Single(w=>w.Name=="MAG_1").ID,
            //        AssortmentID = assortment.Single(a=>a.Name=="FIFA 16 PS4").ID,
            //        Quantity = 35.0
            //    },
            //    new AssortmentWarehouse
            //    {
            //        WarehouseID=warehouses.Single(w=>w.Name=="MAG_1").ID,
            //        AssortmentID = assortment.Single(a=>a.Name=="Driveclub PS4").ID,
            //        Quantity = 20.0
            //    },
            //    new AssortmentWarehouse
            //    {
            //        WarehouseID=warehouses.Single(w=>w.Name=="MAG_1").ID,
            //        AssortmentID = assortment.Single(a=>a.Name=="Ethan: £owca meteorytów PC").ID,
            //        Quantity = 50.0
            //    },
            //    new AssortmentWarehouse
            //    {
            //        WarehouseID=warehouses.Single(w=>w.Name=="MAG_1").ID,
            //        AssortmentID = assortment.Single(a=>a.Name=="Battlefield 4 PS3").ID,
            //        Quantity = 10.0
            //    }
            //};

            //assortmentInWarehouse.ForEach(aw => context.AssortmentInWarehouse.AddOrUpdate(a => a.ID, aw));
            //context.SaveChanges();

        }
    }
}
