using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MassMineSweeper.Models
{
    public class MineFieldFactory
    {

        public static MineSweeperGame CreateMineField(MineSweeperGame model){
            var field = new MineSweeperGame();

            if (model.GameHeight < 10)
                model.GameHeight = 10;
            if (model.GameWidth < 10)
                model.GameWidth = 10;
            if (model.NumMines > model.GameHeight * model.GameWidth / 2)
                model.NumMines = model.GameHeight * model.GameWidth / 2;
            else if (model.NumMines < model.GameHeight * model.GameWidth / 8)
                model.NumMines = model.GameHeight * model.GameWidth / 8;
            if (String.IsNullOrWhiteSpace(model.GameName))
                model.GameName = "New Game";

            field.GameName = model.GameName;
            field.GameHeight = model.GameHeight;
            field.GameWidth = model.GameWidth;
            field.NumMines = model.NumMines;
            field.Tiles = new List<GameTile>();

            //place tiles
            for (int x = 0; x < model.GameWidth; x++)
            {
                for (int y = 0; y < model.GameHeight; y++)
                {
                    GameTile tile = new GameTile() { XPos=x, YPos = y, HasMine=false, IsRevealed = false, IsFlagged = false };

                    //place hollow
                    if (x >= model.GameWidth / 2 - 2 && x <= model.GameWidth / 2 + 2 && y >= model.GameHeight / 2 - 2 && y <= model.GameHeight / 2 + 2)
                        tile.IsRevealed = true;

                    field.Tiles.Add(tile);
                }
            }

            //place mines
            Random rand = new Random();
            int count = 0;
            while (count < model.NumMines)
            {
                GameTile randTile = field.Tiles[rand.Next(field.Tiles.Count)];
                if (randTile.IsRevealed || randTile.HasMine)
                {
                    randTile.HasMine = true;
                    count++;
                }
            }

            //fill revealed tiles
            for (int i = 0; i < field.Tiles.Count; i++)
            {
                field.Tiles[i].IsRevealed = false;
            }

                return field;
        }

    }
}