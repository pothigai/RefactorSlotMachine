using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorSlotMachine
{
    internal class UISlotMachine
    {
        public void printOutputMessage(string message)
        {
            Console.WriteLine(message);
        }

        public string scanInputString()
        {
            return Console.ReadLine();
        }

        public int scanInputInteger(string message)
        {
            int output = 0;
            string input = "";
            while (!int.TryParse(input, out output))
            {
                Console.WriteLine(message);
                input = Console.ReadLine();
                if (!int.TryParse(input, out output))
                {
                    Console.WriteLine("Invalid input, please enter an integer.");
                }
            }
            return output;
        }

        public char scanInputChar(string message)
        {
            char output;
            string input = "";
            while (!char.TryParse(input, out output))
            {
                Console.WriteLine(message);
                input = Console.ReadLine();
                if (!char.TryParse(input, out output))
                {
                    Console.WriteLine("Invalid input, please enter a character.");
                }
            }
            return char.ToLower(output);
        }

        public void clear()
        {
            Console.Clear();
        }
    }
}
