namespace MassMineSweeper.Migrations
{
    using MassMineSweeper.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MassMineSweeper.Models.MassMineSweeperContext>
    {
        UserManager<Member> userManager;
        PasswordHasher hasher = new PasswordHasher();

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "MassMineSweeper.Models.MassMineSweeperContext";
        }

        protected override void Seed(MassMineSweeperContext context)
        {

            userManager = new UserManager<Member>(new UserStore<Member>(context));
            context.Roles.AddOrUpdate(r => r.Name, new IdentityRole("Admin"), new IdentityRole("Alive"));
            context.SaveChanges();
            //create admin account

            InitUser("Admin", "Cooper.t.0043@gmail.com", "IAmTheLaw", "Admin");

            //populate database


            base.Seed(context);
        }

        private Member InitUser(string name, string email, string password, string roleName)
        {

            Member user = new Member()
            {
                UserName = name,
                Email = email,
                PasswordHash = hasher.HashPassword(password)
            };

            Member oldUser = userManager.FindByName(name);

            if (oldUser == null)
            {
                userManager.Create(user, password);
                userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            }
            else
            {
                oldUser.PasswordHash = user.PasswordHash;
                oldUser.Email = user.Email;

                user = oldUser;
            }

            //set role
            userManager.AddToRole(user.Id, roleName);
            userManager.AddToRole(user.Id, "Alive");
            return user;
        }
    }
}
