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
        public int MineSweeperGameID { get; set; }
        public int GamePlayerID { get; set; }//author
        public String GameName { get; set; }
        public DateTime DateCreated { get; set; }
        public TimeSpan RespawnLength { get; set; }
        public int GameHeight { get; set; }
        public int GameWidth { get; set; }
        public int NumMines { get; set; }
        public List<GameTile> Tiles { get; set; }

        public void Initialize(){
            if (GameHeight < 10)
                GameHeight = 10;
            if (GameWidth < 10)
                GameWidth = 10;
            if (NumMines > GameHeight * GameWidth / 2)
                NumMines = GameHeight * GameWidth / 2;
            else if (NumMines < GameHeight * GameWidth / 8)
                NumMines = GameHeight * GameWidth / 8;
            if (String.IsNullOrWhiteSpace(GameName))
                GameName = "New Game";
            Tiles = new List<GameTile>();

            //place tiles
            for (int x = 0; x < GameWidth; x++)
            {
                for (int y = 0; y < GameHeight; y++)
                {
                    GameTile tile = new GameTile() { XPos=x, YPos = y, HasMine=false, IsRevealed = false, IsFlagged = false };

                    //place hollow
                    if (x >= GameWidth / 2 - 1 && x <= GameWidth / 2 + 1 && y >= GameHeight / 2 - 1 && y <= GameHeight / 2 + 1)
                        tile.IsRevealed = true;

                    Tiles.Add(tile);
                }
            }

            //place mines
            Random rand = new Random();
            int count = 0;
            while (count < NumMines)
            {
                GameTile randTile = Tiles[rand.Next(Tiles.Count)];
                if (!(randTile.IsRevealed || randTile.HasMine))
                {
                    randTile.HasMine = true;
                    count++;
                }
            }

            //fill revealed tiles
            for (int i = 0; i < Tiles.Count; i++)
            {
                Tiles[i].IsRevealed = false;
            }

            //reveal center
            RevealTile(GameWidth / 2, GameHeight / 2);
        }


        public GameTile RevealTile(int xPos, int yPos)
        {
            GameTile tile = GetTileByCoordinates(xPos, yPos);
            if (tile != null && !tile.IsRevealed)
            {
                tile.IsRevealed = true;
                tile.IsFlagged = false;
                if (CountSurroundingMines(xPos, yPos) == 0)
                {
                    foreach (GameTile t in GetSurroundingTiles(xPos, yPos))
                    {
                        RevealTile(t.XPos, t.YPos);
                    }
                }
            }

            return tile;

        }

        public GameTile ToggleTileFlag(int xPos, int yPos)
        {
            GameTile tile = GetTileByCoordinates(xPos, yPos);

            if (tile != null && !tile.IsRevealed)
            {
                tile.IsFlagged = !tile.IsFlagged;
            }

            return tile;
        }

        public GameTile GetTileByCoordinates(int x, int y)
        {
            if (x >= GameWidth || x < 0 || y >= GameHeight || y < 0)
            {
                return null;
            }

            if (Tiles != null)
            {
                foreach (GameTile tile in Tiles)
                {
                    if (tile.XPos == x && tile.YPos == y)
                        return tile;
                }
            }

            return null;
        }

        public List<GameTile> GetSurroundingTiles(int xPos, int yPos)
        {
            List<GameTile> tiles = new List<GameTile>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x != 0 || y != 0)
                    {
                        GameTile tile = GetTileByCoordinates(xPos + x, yPos + y);
                        if (tile != null)
                            tiles.Add(tile);
                    }
                }
            }
            return tiles;
        }

        public int CountSurroundingMines(int xPos, int yPos)
        {
            List<GameTile> tiles = GetSurroundingTiles(xPos, yPos);
            int count = 0;
            foreach (GameTile tile in tiles)
            {
                if (tile != null && tile.HasMine)
                    count++;
            }

            return count;
        }

        public int GetRemainingMines()
        {
            int count = 0;
            foreach (GameTile tile in Tiles)
            {
                if ((!tile.IsRevealed && tile.IsFlagged) || (tile.IsRevealed && tile.HasMine))
                    count++;
            }

            return NumMines - count;
        }

        public bool IsGameCleared()
        {
            if (GetRemainingMines() == 0)
            {
                foreach (GameTile tile in Tiles)
                {
                    if (!tile.IsRevealed && !tile.IsFlagged)
                        return false;
                }
                return true;
            }
            return false;
        }

        public string GetTileState(int xPos, int yPos)
        {
            GameTile tile = GetTileByCoordinates(xPos, yPos);

            if (tile == null)
                return "";
            else if (tile.IsRevealed)
            {
                if (tile.HasMine)
                    return "Mine";
                switch (CountSurroundingMines(xPos, yPos))
                {
                    case 0:
                        return "Empty";
                    case 1:
                        return "One";
                    case 2:
                        return "Two";
                    case 3:
                        return "Three";
                    case 4:
                        return "Four";
                    case 5:
                        return "Five";
                    case 6:
                        return "Six";
                    case 7:
                        return "Seven";
                    case 8:
                        return "Eight";
                    default:
                        return "";
                }
            }
            else if (tile.IsFlagged)
                return "Flagged";
            else
                return "Default";
            
        }
    }
}