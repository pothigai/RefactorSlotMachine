namespace RefactorSlotMachine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Choose which lines to play (R = Row, C = Column, D = Diagonal) and then press Q to quit:");

            
            bool[] selectedLines = new bool[3];

            while (true)
            {
                string choice = Console.ReadLine().ToLower();
                Console.WriteLine(); 

                if (choice == "r")
                {
                    selectedLines[0] = true;
                    Console.WriteLine("Row selected.");
                }
                if (choice == "c")
                {
                    selectedLines[1] = true;
                    Console.WriteLine("Column selected.");
                }
                if (choice == "d")
                {
                    selectedLines[2] = true;
                    Console.WriteLine("Diagonal selected.");
                }
                if (choice == "q")
                {
                    
                    Console.WriteLine("Selected lines:");
                    if (selectedLines[0]) Console.WriteLine("Row");
                    if (selectedLines[1]) Console.WriteLine("Column");
                    if (selectedLines[2]) Console.WriteLine("Diagonal");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please select (R), (C), (D), or (Q) to quit.");
                }
            }
        }
    }
}
