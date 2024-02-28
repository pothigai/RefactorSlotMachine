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

        public bool scanInputInteger(string input, out int result)
        {
            return int.TryParse(input, out result);
        }

        public bool scanInputChar(string input, out char result)
        {
            return char.TryParse(input, out result);
        }

        public void clear()
        {
            Console.Clear();
        }
    }
}
