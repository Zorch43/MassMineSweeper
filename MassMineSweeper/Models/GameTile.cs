﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MassMineSweeper.Models
{
    public class GameTile
    {

        [Key]
        public int GameTileID { get; set; }
        public int MineSweeperGameID { get; set; }
        public int XPos { get; set; }
        public int YPos { get; set; }
        public bool HasMine { get; set; }
        public bool IsFlagged { get; set; }
        public bool IsRevealed { get; set; }
    }
}