using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MassMineSweeper.Models
{
    public class TileViewModel
    {
        public int MineSweeperGameID { get; set; }
        public int XPos { get; set; }
        public int YPos { get; set; }
        public string TileState { get; set; }
    }
}