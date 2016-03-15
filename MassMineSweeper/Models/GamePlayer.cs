using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MassMineSweeper.Models
{
    public class GamePlayer
    {
        [Key]
        public int GamePlayerID { get; set; }
        public string MemberID { get; set; }
        public int MineSweeperGameID { get; set; }//current game
        public DateTime? RespawnAt { get; set; }
        public int XPos { get; set; }
        public int YPos { get; set; }
        public bool IsDead { get; set; }
    }
}