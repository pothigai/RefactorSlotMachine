using RefactorSlotMachine;

namespace RefactorSlotMachine
{
    internal class Program
    {
        static void Main(string[] args)
        {

            UISlotMachine UI = new UISlotMachine();
            SlotMachineChecker SC = new SlotMachineChecker();

            //Ask the user for a min buy in of 500 points
            int totalPoints = UI.scanInputInteger($"Please enter your buy in amount, the minimum is {Constants.MIN_BUYIN} points:");

            while (totalPoints < Constants.MIN_BUYIN)
            {
                totalPoints = UI.scanInputInteger($"The entered amount does not meet the minimum buy in amount, please enter an amount of aleast {Constants.MIN_BUYIN}:");
            }

            List<char> selectedLines = new List<char>();

            bool continueAhead = true;

            while (continueAhead)
            {
                char choice = UI.scanInputChar("Choose which lines to play (R = Row, C = Column, D = Diagonal) and then press P to play:");
                UI.printOutputMessage("");

                switch (choice)
                {
                    case Constants.ROW:
                    case Constants.COL:
                    case Constants.DIAG:

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

                    case Constants.PLAY:
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

            char playAgain = Constants.POSITVE_INPUT;

            while (playAgain == Constants.POSITVE_INPUT)
            {
                UI.clear();

                bool reBuy = true;

                (reBuy, totalPoints) = SC.checkTopUp(totalPoints, reBuy);
                
                if (!reBuy)
                {
                    break;
                }

                totalPoints -= Constants.PLAY_COST;

                //Generate the random 3x3 matrix for the slot machine

                int[,] slots = new int[3, 3] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } }; //Use to test code

                //int[,] slots = new int[MATRIX_SIZE, MATRIX_SIZE];

                //for (int i = 0; i < MATRIX_SIZE; i++)
                //{
                //    for (int j = 0; j < MATRIX_SIZE; j++)
                //    {
                //        slots[i, j] = rnd.Next(MAX_NUMBER);
                //    }
                //}

                //Print the matrix to display to the user
                for (int i = 0; i < Constants.MATRIX_SIZE; i++)
                {
                    UI.printOutputMessage("");

                    for (int j = 0; j < Constants.MATRIX_SIZE; j++)
                    {
                        Console.Write("   " + slots[i, j]);
                    }
                }

                UI.printOutputMessage("");
                UI.printOutputMessage("Selected lines:");
                if (selectedLines.Contains(Constants.ROW)) UI.printOutputMessage("Row");
                if (selectedLines.Contains(Constants.COL)) UI.printOutputMessage("Column");
                if (selectedLines.Contains(Constants.DIAG)) UI.printOutputMessage("Diagonal");

                bool rowMatch = false;
                bool colMatch = false;
                bool diagonalMatch = false;
                bool allValuesMatch = true;

                //Check rows for a match in values
                if (selectedLines.Contains(Constants.ROW))
                {
                    (rowMatch, totalPoints) = SC.CheckRowAndColumn(Constants.ROW, slots, totalPoints);
                }

                //Check columns for a match in values
                if (selectedLines.Contains(Constants.COL))
                {
                    (colMatch, totalPoints) = SC.CheckRowAndColumn(Constants.COL, slots, totalPoints);
                }

                //Check diagonals for a match in values
                if (selectedLines.Contains(Constants.DIAG))
                {
                    SC.AwardPointsForDiagonalMatches(slots, ref totalPoints, ref diagonalMatch, Constants.DIAG_POINT);
                }

                //Check all values in matrix
                allValuesMatch = SC.checkAllValues(slots);

                if (allValuesMatch)
                {
                    UI.printOutputMessage($"You won {Constants.JACKPOT} points, all values in the slot match!");
                    totalPoints = totalPoints + Constants.JACKPOT;
                }
                if (!rowMatch && !colMatch && !diagonalMatch && !allValuesMatch)
                {
                    UI.printOutputMessage("You lost!");
                }

                //Display round winnings and ask user if they wish to play again
                UI.printOutputMessage($"Your total points are {totalPoints}");
                playAgain = UI.scanInputChar($"Do you want to play again? {Constants.POSITVE_INPUT}/{Constants.NEGATIVE_INPUT}");

                while (playAgain != Constants.POSITVE_INPUT && playAgain != Constants.NEGATIVE_INPUT)
                {
                    playAgain = UI.scanInputChar($"Invalid input. Please enter '{Constants.POSITVE_INPUT}' or '{Constants.NEGATIVE_INPUT}'.");
                }
            }
        }
    }
}