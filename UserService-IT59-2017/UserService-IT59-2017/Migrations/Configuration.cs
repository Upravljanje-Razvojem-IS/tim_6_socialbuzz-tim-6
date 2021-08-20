namespace UserService_IT59_2017.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using UserService_IT59_2017.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<UserService_IT59_2017.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "UserService_IT59_2017.Models.ApplicationDbContext";
        }

        protected override void Seed(UserService_IT59_2017.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            Role r1 = new Role() { Role_Name = "Administrator" };
            Role r2 = new Role() { Role_Name = "Guest" };
            Role r3 = new Role() { Role_Name = "Dispatcher" };
            Role r4 = new Role() { Role_Name = "Authorized user" };

            context.Roles.AddOrUpdate(
                r1, r2, r3, r4
            );


            context.PersonalAccounts.AddOrUpdate(
                new PersonalAccount()
                {
                    Name = "Marko",
                    Surname = "Markovic",
                    Address = "Mala Mostanica, Velje Nevolje 11",
                    Email = "mare@app.com",
                    PhoneNumber = "+381604829583",
                    Username = "mre93018",
                    Rola = r2
                },
                new PersonalAccount()
                {
                    Name = "Slavica",
                    Surname = "Simic",
                    Address = "Mali Mokri Lug, Djuzepea Zanotija 92",
                    Email = "slavka@app.com",
                    PhoneNumber = "+381604302910",
                    Username = "slvk32418",
                    Rola = r4
                }
            );


            context.CorporateAccounts.AddOrUpdate(
                new CorporateAccount()
                {
                    Name = "Slaven",
                    Surname = "Subasic",
                    Address = "Gornja Koviljaca, Mlade Bosne 16",
                    Email = "slaven@app.com",
                    PhoneNumber = "+38160432411",
                    Username = "slvn19991", 
                    CorporationName = "Invest d.o.o.",
                    Rola = r1
                },
                new CorporateAccount()
                {
                    Name = "Kucura",
                    Surname = "Koceljevic",
                    Address = "Istocno Sirogojno, Slavice Cukteras 18",
                    Email = "kucurakoceljeva@app.com",
                    PhoneNumber = "+38160050394",
                    Username = "kucuraaa32418",
                    CorporationName = "Market commerce d.o.o.",
                    Rola = r3
                }
            );

            base.Seed(context);
        }
    }
}
