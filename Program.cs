namespace RefactorSlotMachine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Initialize constants
            const int HOR_POINT = 20;
            const int COL_POINT = 20;
            const int DIAG_POINT = 30;
            const int JACKPOT = 100;
            const int MIN_BUYIN = 500;
            const int MATRIX_SIZE = 3;
            const int MAX_NUMBER = 10;
            const int PLAY_COST = 50;
            const char POSITVE_INPUT = 'y';
            const char NEGATIVE_INPUT = 'n';

            Random rnd = new Random();

            UISlotMachine UI = new UISlotMachine();

            //Ask the user for a min buy in of 500 points
            int totalPoints = UI.scanInputInteger($"Please enter your buy in amount, the minimum is {MIN_BUYIN} points:");

            while (totalPoints < MIN_BUYIN)
            {
                totalPoints = UI.scanInputInteger($"The entered amount does not meet the minimum buy in amount, please enter an amount of aleast {MIN_BUYIN}:");
            }

            bool[] selectedLines = new bool[3];

            while (true)
            {
                char choice = UI.scanInputChar("Choose which lines to play (R = Row, C = Column, D = Diagonal) and then press P to play:");
                UI.printOutputMessage("");

                if (choice == 'r')
                {
                    selectedLines[0] = true;
                    UI.printOutputMessage("Row selected.");
                }
                if (choice == 'c')
                {
                    selectedLines[1] = true;
                    UI.printOutputMessage("Column selected.");
                }
                if (choice == 'd')
                {
                    selectedLines[2] = true;
                    UI.printOutputMessage("Diagonal selected.");
                }
                if (choice == 'p')
                {

                    UI.printOutputMessage("Selected lines:");
                    if (selectedLines[0]) UI.printOutputMessage("Row");
                    if (selectedLines[1]) UI.printOutputMessage("Column");
                    if (selectedLines[2]) UI.printOutputMessage("Diagonal");
                    break;
                }
                if (choice != 'p' && choice != 'r' && choice != 'c' && choice != 'd')
                {
                    UI.printOutputMessage("Invalid choice. Please select (R), (C), (D), or (P) to play.");
                }
            }

            char playAgain = POSITVE_INPUT;

            while (playAgain == POSITVE_INPUT)
            {
                UI.clear();

                //Checking if user has enough points to play
                if (totalPoints <= 0)
                {
                    char topUp = UI.scanInputChar($"You do not have enough points, would you like to top up? ({POSITVE_INPUT}/{NEGATIVE_INPUT}): ");

                    while (topUp != POSITVE_INPUT && topUp != NEGATIVE_INPUT)
                    {
                        topUp = UI.scanInputChar($"Invalid input, please enter {POSITVE_INPUT}/{NEGATIVE_INPUT}:");
                    }

                    if (topUp == POSITVE_INPUT)
                    {
                        while (totalPoints <= MIN_BUYIN)
                        {
                            totalPoints += UI.scanInputInteger($"You don't meet the minimum of {MIN_BUYIN}, please enter another amount:");
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                totalPoints -= PLAY_COST;

                //Generate the random 3x3 matrix for the slot machine

                int[,] slots = new int[3, 3] { { 2, 2, 8 }, { 2, 2, 2 }, { 2, 2, 9 } }; //Use to test code

                //int[,] slots = new int[MATRIX_SIZE, MATRIX_SIZE];

                //for (int i = 0; i < MATRIX_SIZE; i++)
                //{
                //    for (int j = 0; j < MATRIX_SIZE; j++)
                //    {
                //        slots[i, j] = rnd.Next(MAX_NUMBER);
                //    }
                //}

                //Print the matrix to display to the user
                for (int i = 0; i < MATRIX_SIZE; i++)
                {
                    UI.printOutputMessage("");

                    for (int j = 0; j < MATRIX_SIZE; j++)
                    {
                        Console.Write("   " + slots[i, j]);
                    }
                }

                UI.printOutputMessage("");
                UI.printOutputMessage("Selected lines:");
                if (selectedLines[0]) UI.printOutputMessage("Row");
                if (selectedLines[1]) UI.printOutputMessage("Column");
                if (selectedLines[2]) UI.printOutputMessage("Diagonal");


                bool rowMatch = false;
                bool colMatch = false;
                bool diagonalMatch = false;
                bool allValuesMatch = true;

                //Check rows for a match in values
                if (selectedLines[0])
                {
                    int rowCount;
                    if (rowCheck(slots, out rowCount))
                    {
                        UI.printOutputMessage($"You won, all values in row {rowCount} are a match");
                        totalPoints += HOR_POINT;
                        rowMatch = true;
                    }
                }

                //Check columns for a match in values
                if (selectedLines[1])
                {
                    int colCount;
                    if (colCheck(slots, out colCount))
                    {
                        UI.printOutputMessage($"You won, all values in column {colCount} are a match");
                        totalPoints += COL_POINT;
                        colMatch = true;
                    }
                }

                //Check diagonals for a match in values
                if (selectedLines[2])
                {
                    int diagCount;
                    if (diagonalCheck(slots, out diagCount)[0])
                    {
                        UI.printOutputMessage($"You won, all values in diagonal {diagCount} are a match");
                        totalPoints += DIAG_POINT;
                        diagonalMatch = true;
                    }
                    if (diagonalCheck(slots, out diagCount)[1])
                    {
                        UI.printOutputMessage($"You won, all values in diagonal {diagCount} are a match");
                        totalPoints += DIAG_POINT;
                        diagonalMatch = true;
                    }
                }

                int firstValue = slots[0, 0];
                //Check if all 9 values are a match
                for (int i = 0; i < MATRIX_SIZE; i++)
                {
                    for (int j = 0; j < MATRIX_SIZE; j++)
                    {
                        if (slots[i, j] != firstValue)
                        {
                            allValuesMatch = false;
                            break;
                        }
                    }
                    if (!allValuesMatch)
                    {
                        break;
                    }
                }

                if (allValuesMatch)
                {
                    UI.printOutputMessage($"You won {JACKPOT} points, all values in the slot match!");
                    totalPoints = totalPoints + JACKPOT;
                }
                if (!rowMatch && !colMatch && !diagonalMatch && !allValuesMatch)
                {
                    UI.printOutputMessage("You lost!");
                }

                //Display round winnings and ask user if they wish to play again
                UI.printOutputMessage($"Your total points are {totalPoints}");
                playAgain = UI.scanInputChar($"Do you want to play again? {POSITVE_INPUT}/{NEGATIVE_INPUT}");

                while (playAgain != POSITVE_INPUT && playAgain != NEGATIVE_INPUT)
                {
                    playAgain = UI.scanInputChar($"Invalid input. Please enter '{POSITVE_INPUT}' or '{NEGATIVE_INPUT}'.");
                }
            }
        }
        static bool rowCheck(int[,] inputMatrix, out int rowCount)
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

        static bool colCheck(int[,] inputMatrix, out int colCount)
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

        static bool[] diagonalCheck(int[,] inputMatrix, out int diagCount)
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
