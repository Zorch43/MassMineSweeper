using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MassMineSweeper.Models
{
    public class MinesweeperDBInitializer : DropCreateDatabaseAlways<MassMineSweeperContext>
    {
         protected override void Seed(MassMineSweeperContext context)
        {
            //create player accounts
            GamePlayer player1 = new GamePlayer()
            {
                UserName = "Player_One",
                Email = "Aceinthehole@yahoo.com",
                Password = "Pass_word1",
                XPos = 0,
                YPos = 0,
                IsAdmin = false,
                MineSweeperGameId = null,
                RespawnAt = null
            };

            GamePlayer player2 = new GamePlayer()
            {
                UserName = "Player_A",
                Email = "Admin@MassMineSweeper.com",
                Password = "IAmTheLaw",
                XPos = 0,
                YPos = 0,
                IsAdmin = true,
                MineSweeperGameId = null,
                RespawnAt = null
            };

            //create games
            MineSweeperGame game1 = new MineSweeperGame()
            {
                GameName = "Test Game",
                GameHeight = 100,
                GameWidth = 100,
                NumMines = 20,
                RespawnLength = new TimeSpan(18,0,0),
                DateCreated = DateTime.Now,
                Tiles = null
            };

            MineSweeperGame game2 = new MineSweeperGame()
            {
                GameName = "Mega-Mines",
                GameHeight = 1000,
                GameWidth = 1000,
                NumMines = 200000,
                RespawnLength = new TimeSpan(1, 0, 0),
                DateCreated = DateTime.Now,
                Tiles = null
            };

            //populate database
            context.GamePlayers.Add(player1);
            context.GamePlayers.Add(player2);
            context.MineSweeperGames.Add(game1);
            context.MineSweeperGames.Add(game2);

            base.Seed(context);
         }
        
    }
}