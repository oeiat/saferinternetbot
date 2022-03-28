using System;
using System.Data.Entity.Migrations;
using System.Linq;
using oiat.saferinternetbot.DataAccess.Entities;
using oiat.saferinternetbot.DataAccess.Enums;

namespace oiat.saferinternetbot.DataAccess.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DataContext context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
