using RefactorSlotMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace RefactorSlotMachine
{
    public class SlotMachineChecker
    {
        UISlotMachine UI = new UISlotMachine();
        public bool checkRowResults(int[,] inputMatrix, out bool[] matchingRows)
        {
            matchingRows = new bool[3];

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
                    matchingRows[i] = true;
                }
            }

            return matchingRows.Contains(true);
        }

        public bool checkColumnResults(int[,] inputMatrix, out bool[] matchingRows)
        {
            matchingRows = new bool[Constants.MATRIX_SIZE];

            for (int i = 0; i < 3; i++)
            {
                int colValue = inputMatrix[0, i];
                bool allColSame = true;

                for (int j = 1; j < 3; j++)
                {
                    if (inputMatrix[j, i] != colValue)
                    {
                        allColSame = false;
                        break;
                    }
                }

                if (allColSame)
                {
                    matchingRows[i] = true;
                }
            }

            return matchingRows.Contains(true);
        }

        public List<int> checkDiagonalResults(int[,] inputMatrix)
        {
            bool diagonal1 = true;
            bool diagonal2 = true;

            List<int> diagCounts = new List<int>();

            for (int i = 1; i < 3; i++)
            {
                if (inputMatrix[0, 0] != inputMatrix[i, i])
                {
                    diagonal1 = false;
                }
                if (inputMatrix[0, 2] != inputMatrix[i, 2 - i])
                {
                    diagonal2 = false;
                }
            }

            if (diagonal1)
            {
                diagCounts.Add(1);
            }
            if (diagonal2)
            {
                diagCounts.Add(2);
            }
            return diagCounts;
        }

        public void AwardPointsForDiagonalMatches(int[,] slots, ref int totalPoints, ref bool diagonalMatch, int DIAG_POINT)
        {
            List<int> diagCounts = checkDiagonalResults(slots);
            foreach (int diagCount in diagCounts)
            {
                UI.printOutputMessage($"You won, all values in diagonal {diagCount} are a match");
                totalPoints += DIAG_POINT;
                diagonalMatch = true;
            }
        }

        public (bool, int) CheckRowAndColumn(char type, int[,] slots, int totalPoints)
        {
            bool match = false;
            bool[] matchingLines = new bool[3];
            int points = 0;
            string line = "";

            if (type == Constants.ROW)
            {
                match = checkRowResults(slots, out matchingLines);
                points = Constants.ROW_POINT;
                line = "row";
            }

            else if (type == Constants.COL)
            {
                match = checkColumnResults(slots, out matchingLines);
                points = Constants.COL_POINT;
                line = "column";
            }

            if (match)
            {
                string lines = "";
                for (int i = 0; i < matchingLines.Length; i++)
                {
                    if (matchingLines[i])
                    {
                        lines += (i + 1).ToString();
                        if (i < matchingLines.Length - 1)
                        {
                            lines += ",";
                        }
                    }
                }

                UI.printOutputMessage($"You won, all values are a match in {line}/s: " + lines);
                match = true;
                for (int i = 0; i < matchingLines.Length; i++)
                {
                    if (matchingLines[i])
                    {
                        totalPoints += points;
                    }
                }
            }

            return (match, totalPoints);
        }

    }
}