namespace proiect_ProgramareAvansataPePlatforma.NET.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<proiect_ProgramareAvansataPePlatforma.NET.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "proiect_ProgramareAvansataPePlatforma.NET.Models.ApplicationDbContext";
        }

        protected override void Seed(proiect_ProgramareAvansataPePlatforma.NET.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
