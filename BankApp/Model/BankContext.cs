
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PRBD_Framework;
namespace BankApp.Model
{
    public class BankContext : DbContextBase
    {
       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                .LogTo(Console.WriteLine, LogLevel.Information)
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=BankApp")
                .UseLazyLoadingProxies(true);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AccessClient>()
                .HasKey(a => new { a.Iban, a.Pseudo });
            /* Relation Client et Access  */
             modelBuilder.Entity<AccessClient>()
                .HasOne(a => a.Client)
                .WithMany(c => c.AccessClients)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<Client>()
               .HasMany(c => c.AccessClients)
               .WithOne(a => a.Client)
               .OnDelete(DeleteBehavior.ClientCascade);

            /* Relation Access et IntAccount */
            modelBuilder.Entity<AccessClient>()
                .HasOne(a => a.IntAccount)
                .WithMany(i => i.AccessClients)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<IntAccount>()
                .HasMany(i => i.AccessClients)
                .WithOne(a => a.IntAccount)
                .OnDelete(DeleteBehavior.ClientCascade);
           

           /*  Relation Transfer et Account  */

            modelBuilder.Entity<Transfer>()
                .HasOne(a => a.AccountPayer)
                .WithMany(t => t.TransfersOut)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<Transfer>()
                .HasOne(a => a.AccountCreditor)
                .WithMany(t => t.TransfersIn)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<Account>()
                .HasMany(a => a.TransfersIn)
                .WithOne(t => t.AccountCreditor)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<Account>()
                .HasMany(a => a.TransfersOut)
                .WithOne(t => t.AccountPayer)
                .OnDelete(DeleteBehavior.ClientCascade);
            /* Discriminator  */
            modelBuilder.Entity<User>()
                .HasDiscriminator(u => u.Role)
                .HasValue<Client>(Role.Client)
                .HasValue<Admin>(Role.Admin)
                .HasValue<Manager>(Role.Manager);
            /*  Relation Agences et Manager */
            modelBuilder.Entity<Manager>()
                .HasMany(m => m.Agences)
                .WithOne(a => a.Manager)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<Agence>()
                .HasOne(a => a.Manager)
                .WithMany(m => m.Agences)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Transfers)
                .WithOne(t => t.Category)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<Transfer>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Transfers)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<ClientAgencies>()
               .HasKey(a => new { a.Agency, a.Pseudo });
            modelBuilder.Entity<ClientAgencies>()
                .HasOne(ca => ca.Agence)
                .WithMany(a => a.ListClients)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<Agence>()
                .HasMany(a => a.ListClients)
                .WithOne(ca => ca.Agence)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<ClientAgencies>()
                .HasOne(ca => ca.Client)
                .WithOne(c => c.ClientAgencies)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<Client>()
                .HasOne(c => c.ClientAgencies)
                .WithOne(ca => ca.Client)
                .OnDelete(DeleteBehavior.ClientCascade);
            




        }
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins{ get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Agence> Agences { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<IntAccount> IntAccounts { get; set; }
        public DbSet<ExtAccount> ExtAccounts { get; set; }
        public DbSet<CheckingAccount> CheckingAccounts { get; set; }
        public DbSet<SavingAccount> SavingAccounts { get; set; }
        public DbSet<AccessClient> AccessClients { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ClientAgencies> ClientAgencies { get; set; }
        

        public void SeedData()
        {
           
            Database.BeginTransaction();
            var manager1 = new Manager {Pseudo="bruno",Password="bruno",Name="LaCroix",FirstName="Bruno",NbUser=1 };
            var manager2 = new Manager { Pseudo = "ben", Password = "ben", Name = "Penelle", FirstName = "Ben", NbUser = 2 };
            var admin1 = new Admin { Pseudo = "admin", Password = "admin", Name = "Strator", FirstName = "Admin", NbUser = 3 }; ;
            var client1 = new Client { Pseudo = "bob", Password = "bob", Name = "Marley", FirstName = "Bob", NbUser = 4 }; 
            var client2 = new Client { Pseudo = "caro", Password = "caro", Name = "de Monaco", FirstName = "Caroline", NbUser = 5 }; 
            var client3 = new Client { Pseudo = "louise", Password = "louise", Name = "TheCross", FirstName = "Julie", NbUser = 6 };
            var client4 = new Client { Pseudo = "jules", Password = "jules", Name = "TheCross", FirstName = "Jules", NbUser = 7 };


            Users.AddRange(manager1, manager2, client1, client2, admin1,client3,client4);
            var ag1 = new Agence { Name = "Agency1", Manager=manager2 ,NrAgence=1};
            var ag2 = new Agence { Name = "Agency2", Manager = manager2, NrAgence = 2 };
            var ag3 = new Agence { Name = "Agency3", Manager = manager1, NrAgence = 3 };
            Agences.AddRange(ag1,ag2,ag3);
            var cA1 = new CheckingAccount { IBAN= "BE02 9999 1017 8207", Floor = -50, Descriptive="AAA",CreationTime= new DateTime(2021,12,31) };
            var cA2 = new CheckingAccount { IBAN="BE14 9996 1669 4306",Descriptive="BBB",Floor=-10, CreationTime = new DateTime(2021, 12, 31) };
            var cA3 = new CheckingAccount { IBAN = "BE55 9999 6717 9982", Descriptive = "DDD", Floor = -100, CreationTime = new DateTime(2021, 12, 31) };
            var sA1 = new SavingAccount { IBAN="BE71 9991 5987 4787" ,Descriptive = "CCC", CreationTime = new DateTime(2021, 12, 31) };
            var ex1 = new ExtAccount {IBAN="BE23 0081 6870 0358" ,Descriptive = "EEE" };
            Accounts.AddRange(cA1,cA2,cA3,sA1,ex1);
            var ac1 = new ClientAgencies { Agency = "Agency1", Pseudo = "bob" };
            var ac2 = new ClientAgencies { Agency = "Agency1", Pseudo = "caro" };
            var ac3 = new ClientAgencies { Agency = "Agency2", Pseudo = "louise" };
            var ac4 = new ClientAgencies { Agency = "Agency2", Pseudo = "jules" };
            ClientAgencies.AddRange(ac1,ac2,ac3,ac4);
            var access1 = new AccessClient { Iban= "BE02 9999 1017 8207", Pseudo="bob", Access = Access.Holder };
            var access2 = new AccessClient { Iban = "BE14 9996 1669 4306", Pseudo = "bob", Access = Access.Holder };
            var access3 = new AccessClient { Iban= "BE71 9991 5987 4787", Pseudo = "bob", Access = Access.Agent };
            var access4 = new AccessClient { Iban = "BE02 9999 1017 8207", Pseudo = "caro", Access = Access.Agent };
            var access5 = new AccessClient { Iban = "BE55 9999 6717 9982", Pseudo = "caro", Access = Access.Holder};
            var access6 = new AccessClient { Iban = "BE71 9991 5987 4787", Pseudo = "caro", Access = Access.Holder };
            AccessClients.AddRange(access1,access2,access3,access4,access5,access6);
            var cat1 = new Category { Name = "Category 1" };
            var cat2 = new Category { Name = "Category 2" };
            var cat3 = new Category { Name = "Category 3" };
            var cat4 = new Category { Name = "Category 4" };
            var cat5 = new Category { Name = "Category 5" };
            Categories.AddRange(cat1, cat2, cat3,cat4,cat5);
            var tr1 = new Transfer { AccountPayer = cA1, AccountCreditor = cA2, Amount = 10, Descriptive = "Tx #001", CreationTime = new DateTime(2022, 01, 01),  Payer=client1 };
            var tr2 = new Transfer { AccountPayer = cA1, AccountCreditor = sA1, Amount = 5, Descriptive = "Tx #002", CreationTime = new DateTime(2022,01,01),EffectiveTime=new DateTime(2022,01,05),Payer=client2 };
            var tr3 = new Transfer { AccountPayer = cA1, AccountCreditor = cA2, Amount = 35, Descriptive = "Tx #003", CreationTime = new DateTime(2022, 01, 01), EffectiveTime = new DateTime(2022, 01, 09), Payer = client2 }; 
            var tr4 = new Transfer { AccountPayer = cA2, AccountCreditor =sA1, Amount = 15, Descriptive = "Tx #004", CreationTime = new DateTime(2022, 01, 02), EffectiveTime = new DateTime(2022, 01, 03), Payer = client1 };
            var tr5 = new Transfer { AccountPayer = cA1, AccountCreditor = cA2, Amount = 50, Descriptive = "Tx #005", CreationTime = new DateTime(2022, 01, 02), EffectiveTime = new DateTime(2022, 01, 04), Payer = client1 };
            var tr6 = new Transfer { AccountPayer = cA1, AccountCreditor = cA1, Amount = 20, Descriptive = "Tx #006", CreationTime = new DateTime(2022, 01, 03), EffectiveTime = new DateTime(2022, 01, 07), Payer = client1 };
            var tr7 = new Transfer { AccountPayer = ex1, AccountCreditor = sA1, Amount = 5, Descriptive = "Tx #007", CreationTime = new DateTime(2022, 01, 04), EffectiveTime = new DateTime(2022, 01, 08) };
            var tr8 = new Transfer { AccountPayer = sA1, AccountCreditor = cA2, Amount = 100, Descriptive = "Tx #008", CreationTime = new DateTime(2022, 01, 06), Payer = client2 };
            var tr9 = new Transfer { AccountPayer = sA1, AccountCreditor = ex1, Amount = 10, Descriptive = "Tx #009", CreationTime = new DateTime(2022, 01, 07), EffectiveTime = new DateTime(2022, 01, 11), Payer = client1 };
            var tr10 = new Transfer { AccountPayer = sA1, AccountCreditor = cA1, Amount = 15, Descriptive = "Tx #010", CreationTime = new DateTime(2022, 01, 10), Payer = client1 };
            var tr11 = new Transfer { AccountPayer = cA1, AccountCreditor = sA1, Amount = 15, Descriptive = "Tx #011", CreationTime = new DateTime(2022, 01, 11), EffectiveTime = new DateTime(2022, 01, 16), Payer = client2 };
            var tr12 = new Transfer { AccountPayer = cA2, AccountCreditor = sA1, Amount = 35, Descriptive = "Tx #012", CreationTime = new DateTime(2022, 01, 12), EffectiveTime = new DateTime(2022, 01, 14), Payer = client1};
            var tr13 = new Transfer { AccountPayer = cA1, AccountCreditor = sA1, Amount = 100, Descriptive = "Tx #013", CreationTime = new DateTime(2022, 01, 13), Payer = client2 };
            var tr14 = new Transfer { AccountPayer = cA1, AccountCreditor = ex1, Amount = 15, Descriptive = "Tx #014", CreationTime = new DateTime(2022, 01, 15),Payer=manager2};

            Transfers.AddRange(tr1, tr2, tr3,tr4,tr5,tr6,tr7,tr8,tr9,tr10,tr11,tr12,tr13,tr14);
            /* "21/04/2022 17:36:59" */






            SaveChanges();
           
            Database.CommitTransaction();

      
        }



    }
}
