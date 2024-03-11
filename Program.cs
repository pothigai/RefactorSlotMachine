namespace RefactorSlotMachine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Initialize constants
            const int ROW_POINT = 20;
            const int COL_POINT = 20;
            const int DIAG_POINT = 30;
            const int JACKPOT = 100;
            const int MIN_BUYIN = 500;
            const int MATRIX_SIZE = 3;
            const int PLAY_COST = 50;
            const char POSITVE_INPUT = 'y';
            const char NEGATIVE_INPUT = 'n';
            const char ROW = 'r';
            const char COL = 'c';
            const char DIAG = 'd';
            const char PLAY = 'p';

            UISlotMachine UI = new UISlotMachine();
            SlotMachineChecker SC = new SlotMachineChecker();

            //Ask the user for a min buy in of 500 points
            int totalPoints = UI.scanInputInteger($"Please enter your buy in amount, the minimum is {MIN_BUYIN} points:");

            while (totalPoints < MIN_BUYIN)
            {
                totalPoints = UI.scanInputInteger($"The entered amount does not meet the minimum buy in amount, please enter an amount of aleast {MIN_BUYIN}:");
            }

            List<char> selectedLines = new List<char>();
            bool continueAhead = true;
            while (continueAhead)
            {
                char choice = UI.scanInputChar("Choose which lines to play (R = Row, C = Column, D = Diagonal) and then press P to play:");
                UI.printOutputMessage("");

                switch (choice)
                {
                    case ROW:
                    case COL:
                    case DIAG:

                        if (!selectedLines.Contains(choice))
                        {
                            selectedLines.Add(choice);
                            UI.printOutputMessage($"{choice} selected.");
                        }
                        else
                        {
                            UI.printOutputMessage($"{choice} is already selected.");
                        }
                        break;

                    case PLAY:
                        UI.printOutputMessage("Selected lines:");
                        foreach (char line in selectedLines)
                        {
                            UI.printOutputMessage($"{line}");
                            continueAhead = false;
                        }
                        break;

                    default:
                        UI.printOutputMessage("Invalid choice. Please select (R), (C), (D), or (P) to play.");
                        break;
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

                int[,] slots = new int[3, 3] { { 2, 3, 2 }, { 3, 2, 2 }, { 2, 3, 2 } }; //Use to test code

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
                if (selectedLines.Contains(ROW)) UI.printOutputMessage("Row");
                if (selectedLines.Contains(COL)) UI.printOutputMessage("Column");
                if (selectedLines.Contains(DIAG)) UI.printOutputMessage("Diagonal");


                bool rowMatch = false;
                bool colMatch = false;
                bool diagonalMatch = false;
                bool allValuesMatch = true;

                bool CheckRowAndColumn(List<char> selectedLines, int[,] slots, string type)
                {
                    bool match = false;
                    bool[] matchingLines = new bool[3];
                    int points = 0;

                    if (type == "row")
                    {
                        match = SC.checkRowResults(slots, out matchingLines);
                        points = ROW_POINT;
                    }

                    else if (type == "column")
                    {
                        match = SC.checkColumnResults(slots, out matchingLines);
                        points = COL_POINT;
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

                        UI.printOutputMessage($"You won, all values are a match in {type}/s: " + lines);
                        match = true;
                        for (int i = 0; i < matchingLines.Length; i++)
                        {
                            if (matchingLines[i])
                            {
                                totalPoints += points;
                            }
                        }
                    }

                    return match;
                }

                void AwardPointsForDiagonalMatches(int[,] slots, ref int totalPoints, ref bool diagonalMatch)
                {
                    List<int> diagCounts = SC.checkDiagonalResults(slots);
                    foreach (int diagCount in diagCounts)
                    {
                        UI.printOutputMessage($"You won, all values in diagonal {diagCount} are a match");
                        totalPoints += DIAG_POINT;
                        diagonalMatch = true;
                    }
                }
                //Check rows for a match in values
                if (selectedLines.Contains(ROW))
                {
                    rowMatch = CheckRowAndColumn(selectedLines, slots, "row");
                }

                //Check columns for a match in values
                if (selectedLines.Contains(COL))
                {
                    colMatch = CheckRowAndColumn(selectedLines, slots, "column");
                }

                //Check diagonals for a match in values
                if (selectedLines.Contains(ROW))
                {
                    AwardPointsForDiagonalMatches(slots, ref totalPoints, ref diagonalMatch);
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
    }
}
