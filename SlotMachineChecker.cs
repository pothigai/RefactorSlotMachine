using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorSlotMachine
{
    internal class SlotMachineChecker
    {

        public bool checkRowResults(int[,] inputMatrix, out int rowCount)
        {
            bool rowMatch = false;
            rowCount = 0;
            for (int i = 0; i < 3; i++)
            {
                int rowValue = inputMatrix[i, 0];

                bool allRowSame = true;

                for (int j = 1; j < 3; j++)
                {
                    if (inputMatrix[i, j] != rowValue)
                    {
                        allRowSame = false;
                        break;
                    }
                }
                if (allRowSame)
                {
                    rowMatch = true;
                    rowCount = i + 1;
                }
            }
            return rowMatch;
        }

        public bool checkColumnResults(int[,] inputMatrix, out int colCount)
        {
            bool colMatch = false;
            colCount = 0;
            for (int i = 0; i < 3; i++)
            {
                int colValue = inputMatrix[0, i];

                bool allColSame = true;

                for (int j = 0; j < 3; j++)
                {
                    if (inputMatrix[j, i] != colValue)
                    {
                        allColSame = false;
                        break;
                    }
                }
                if (allColSame)
                {
                    colMatch = true;
                    colCount = i + 1;
                }
            }
            return colMatch;
        }

        public bool[] checkDiagonalResults(int[,] inputMatrix, out int diagCount)
        {

            bool[] diagonals = new bool[2];
            diagonals[0] = true;
            diagonals[1] = true;

            diagCount = 0;

            for (int i = 1; i < 3; i++)
            {
                if (inputMatrix[0, 0] != inputMatrix[i, i])
                {
                    diagonals[0] = false;
                }
                if (inputMatrix[0, 2] != inputMatrix[i, 2 - i])
                {
                    diagonals[1] = false;
                }
            }

            if (diagonals[0])
            {
                diagCount = 1;
            }
            if (diagonals[1])
            {
                diagCount = 2;
            }
            return diagonals;
        }
    }
}
