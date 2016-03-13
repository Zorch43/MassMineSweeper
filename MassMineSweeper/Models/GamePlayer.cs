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
        public int GamePlayerId { get; set; }
        public int? MineSweeperGameId { get; set; }//current game
        public String UserName { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public DateTime? RespawnAt { get; set; }
        public int XPos { get; set; }
        public int YPos { get; set; }
        public bool IsAdmin { get; set; }

    }
}