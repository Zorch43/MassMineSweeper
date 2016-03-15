using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MassMineSweeper.Models
{
    public class Member : IdentityUser
    {

        public GamePlayer Player { get; set; }
    }
}