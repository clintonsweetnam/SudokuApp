using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sudoku.Types
{
    public class Board
    {
        //Lock Object
        private object obj = new object();

        public int[,] Tiles { get; set;}

        public Board()
        {
            Tiles = new int[9, 9];
        }

        public bool TrySet(int value, int XPos, int YPos)
        {
            lock (obj)
            {
                var result = IsValidMove(value, XPos, YPos);

                if (result)
                {
                    Tiles[XPos, YPos] = value;
                }

                return result;
            }
        }

        public bool IsValidMove(int value, int xPos, int yPos)
        {
            var result = false;

            if (value >= 1 && value <= 9)
                if(Tiles[xPos, yPos] == 0)
                    if(!ExistsOnTheRowOrColumn(value, yPos, xPos))
                        if(!IsInArea(value, yPos, xPos))
                            result = true;

            return result;
        }

        #region
        private bool IsInArea(int value, int yPos, int xPos)
        {
            var result = false;

            var xArea = xPos / 3;
            var yArea = yPos / 3;

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    if(Tiles[(xArea * 3) + i, (yArea * 3) + j] == value)
                    {
                        result = true;
                        goto end;
                    }
                }
            }

            end:

            return result;
        }

        private bool ExistsOnTheRowOrColumn(int value, int yPos, int xPos)
        {
            var result = false;

            for(int i = 0;i<9;i++)
            {
                if(Tiles[i, yPos] == value
                    || Tiles[xPos, i] == value)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }
        #endregion

    }
}
