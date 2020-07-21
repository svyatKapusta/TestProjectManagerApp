using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TestProjectManagerApp.Models;

namespace TestProjectManagerApp.Data
{
    public class TestProjectManagerAppContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public TestProjectManagerAppContext() : base("name=TestProjectManagerAppContext")
        {
            Database.SetInitializer(new Initializer());
        }



        public System.Data.Entity.DbSet<TestProjectManagerApp.Models.User> Users { get; set; }
        public System.Data.Entity.DbSet<TestProjectManagerApp.Models.Project> Projects { get; set; }
        public System.Data.Entity.DbSet<TestProjectManagerApp.Models.Role> Roles { get; set; }
        public System.Data.Entity.DbSet<TestProjectManagerApp.Models.File> Files { get; set; }


    }
}
