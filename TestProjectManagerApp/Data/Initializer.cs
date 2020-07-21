using System.Collections.Generic;
using System.Data.Entity;
using TestProjectManagerApp.Data;
using TestProjectManagerApp.Models;

public class Initializer : DropCreateDatabaseIfModelChanges<TestProjectManagerAppContext>
{
    protected override void Seed(TestProjectManagerAppContext context)
    {
        List<Role> roles = new List<Role>
        {
            new Role {Id=1, Name="Admin"},
            new Role {Id=2, Name="User"},
        };
        context.Roles.AddRange(roles);
        context.SaveChanges();

        List<User> users = new List<User>
        {
            new User { UserName="Admin", UserId="admin", Password="admin", RoleId=1 },
                new User { UserName = "User", UserId = "user", Password = "user", RoleId = 2 },
        };
        context.Users.AddRange(users);
        context.SaveChanges();

    }
}