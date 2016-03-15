using MassMineSweeper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MassMineSweeper.Controllers
{
    public class MineSweeperLogic
    {
        MineSweeperGame gameModel;

        public MineSweeperLogic() 
            : this(MineFieldFactory.CreateMineField(new MineSweeperGame()))
        { 
            
        }

        public MineSweeperLogic(MineSweeperGame game)
        {
            gameModel = game;
            //reveal center
            RevealTile(game.GameWidth / 2, game.GameHeight / 2);
        }

        public GameTile RevealTile(int xPos, int yPos)
        {
            GameTile tile = gameModel.GetTileByCoordinates(xPos, yPos);
            if (tile != null && !tile.IsRevealed)
            {
                tile.IsRevealed = true;
                tile.IsFlagged = false;
                if (gameModel.CountSurroundingMines(xPos, yPos) == 0)
                {
                    foreach (GameTile t in gameModel.GetSurroundingTiles(xPos, yPos))
                    {
                        RevealTile(t.XPos, t.YPos);
                    }
                }
            }

            return tile;

        }

        public GameTile ToggleTileFlag(int xPos, int yPos)
        {
            GameTile tile = gameModel.GetTileByCoordinates(xPos, yPos);

            if (tile != null && !tile.IsRevealed)
            {
                tile.IsFlagged = !tile.IsFlagged;
            }

            return tile;
        }
    }
}