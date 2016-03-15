using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MassMineSweeper.Models
{
    public class MinesweeperDBInitializer : DropCreateDatabaseAlways<MassMineSweeperContext>
    {
        UserManager<Member> userManager;
        PasswordHasher hasher = new PasswordHasher();

         protected override void Seed(MassMineSweeperContext context)
        {

            userManager = new UserManager<Member>(new UserStore<Member>(context));
            context.Roles.Add(new IdentityRole("Admin"));
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
                 /*var result = */
                 userManager.Create(user, password);
                 userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                 /*if (result.Succeeded)
                 {
                     var identity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

                    
                 }*/
             }
             else
             {
                 oldUser.PasswordHash = user.PasswordHash;
                 oldUser.Email = user.Email;

                 user = oldUser;
             }

             //set role
             userManager.AddToRole(user.Id, roleName);

             return user;
         }
        
    }
}