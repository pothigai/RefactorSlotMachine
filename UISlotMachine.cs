using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorSlotMachine
{
    internal class UISlotMachine
    {
        public void print(string message)
        {
            Console.WriteLine(message);
        }

        public string scan()
        {
            string message = Console.ReadLine();
            return message;
        }

        public void clear()
        {
            Console.Clear();
        }
    }
}
