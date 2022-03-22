namespace SportStore.Domain.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SportStore.Domain.Concrete.EFDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationsEnabled = true;
            ContextKey = "SportStore.Domain.Concrete.EFDBContext";
        }

        protected override void Seed(SportStore.Domain.Concrete.EFDBContext context)
        {
  
        }
    }
}
