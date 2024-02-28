namespace RefactorSlotMachine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Initialize constants
            const int HOR_POINT = 20;
            const int DIAG_POINT = 30;
            const int JACKPOT = 100;
            const int MIN_BUYIN = 500;
            const int MATRIX_SIZE = 3;
            const int MAX_NUMBER = 10;
            const int PLAY_COST = 50;
            const string POSITVE_INPUT = "y";
            const string NEGATIVE_INPUT = "n";

            Random rnd = new Random();

            UISlotMachine UI = new UISlotMachine();

            //Ask the user for a min buy in of 500 points
            UI.printOutputMessage($"Please enter your buy in amount, the minimum is {MIN_BUYIN} points:");

            string playerInput = UI.scanInputString();
            int totalPoints;

            while (!UI.scanInputInteger(playerInput, out totalPoints) || totalPoints < MIN_BUYIN)
            {
                UI.printOutputMessage($"The entered amount either does not meet the minimum buy in amount or is not a valid input, please enter a valid amount of aleast {MIN_BUYIN}:");
                playerInput = UI.scanInputString();
            }

            UI.printOutputMessage("Choose which lines to play (R = Row, C = Column, D = Diagonal) and then press P to play:");


            bool[] selectedLines = new bool[3];

            while (true)
            {
                char choice;  
                UI.scanInputChar(UI.scanInputString(), out choice);
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
                if (choice != 'p' || choice != 'r' || choice != 'c' || choice != 'd')
                {
                    UI.printOutputMessage("Invalid choice. Please select (R), (C), (D), or (P) to play.");
                }
            }

            string playAgain = POSITVE_INPUT;

            while (playAgain == POSITVE_INPUT)
            {
                UI.clear();

                //Checking if user has enough points to play
                if (totalPoints <= 0)
                {
                    UI.printOutputMessage($"You do not have enough points, would you like to top up? ({POSITVE_INPUT}/{NEGATIVE_INPUT}): ");
                    string topUp = UI.scanInputString();

                    while (topUp.ToLower() != POSITVE_INPUT && topUp.ToLower() != NEGATIVE_INPUT)
                    {
                        UI.printOutputMessage($"Invalid input, please enter {POSITVE_INPUT}/{NEGATIVE_INPUT}:");
                        topUp = UI.scanInputString();
                    }

                    if (topUp.ToLower() == POSITVE_INPUT)
                    {
                        UI.printOutputMessage("Please enter the amount of points you want to purchase");
                        totalPoints += Convert.ToInt32(UI.scanInputString());
                    }
                    else
                    {
                        break;
                    }
                }

                totalPoints -= PLAY_COST;

                //Generate the random 3x3 matrix for the slot machine

                //int[,] slots = new int[3, 3] { { 2, 4, 4 }, { 2, 2, 2 }, { 9, 6, 2 } }; //Use to test code

                int[,] slots = new int[MATRIX_SIZE, MATRIX_SIZE];

                for (int i = 0; i < MATRIX_SIZE; i++)
                {
                    for (int j = 0; j < MATRIX_SIZE; j++)
                    {
                        slots[i, j] = rnd.Next(MAX_NUMBER);
                    }
                }

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
                if (!rowMatch && !diagonalMatch && !allValuesMatch)
                {
                    UI.printOutputMessage("You lost!");
                }

                //Display round winnings and ask user if they wish to play again
                UI.printOutputMessage($"Your total points are {totalPoints}");
                UI.printOutputMessage($"Do you want to play again? {POSITVE_INPUT}/{NEGATIVE_INPUT}");
                playAgain = UI.scanInputString();

                while (playAgain.ToLower() != POSITVE_INPUT && playAgain.ToLower() != NEGATIVE_INPUT)
                {
                    UI.printOutputMessage($"Invalid input. Please enter '{POSITVE_INPUT}' or '{NEGATIVE_INPUT}'.");
                    playAgain = UI.scanInputString();
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
