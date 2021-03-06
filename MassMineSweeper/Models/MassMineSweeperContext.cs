﻿using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MassMineSweeper.Models
{
    public class MassMineSweeperContext : IdentityDbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public MassMineSweeperContext() : base("name=MassMineSweeperContext")
        {
        }

        public System.Data.Entity.DbSet<MassMineSweeper.Models.GameTile> GameTiles { get; set; }
        public System.Data.Entity.DbSet<MassMineSweeper.Models.GamePlayer> GamePlayers { get; set; }
        public System.Data.Entity.DbSet<MassMineSweeper.Models.MineSweeperGame> MineSweeperGames { get; set; }
    
    }
}
