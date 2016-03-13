using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MassMineSweeper.Models
{
    public class MineSweeperGame
    {
        [Key]
        public int MineSweeperGameId { get; set; }
        public int GamePlayerId { get; set; }//author
        public String GameName { get; set; }
        public DateTime DateCreated { get; set; }
        public TimeSpan RespawnLength { get; set; }
        public int GameHeight { get; set; }
        public int GameWidth { get; set; }
        public int NumMines { get; set; }
        public List<GameTile> Tiles { get; set; }
    }
}