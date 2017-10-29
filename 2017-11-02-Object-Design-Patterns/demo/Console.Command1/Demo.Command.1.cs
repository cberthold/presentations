using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Command1
{
    class Program
    {
        static void Main(string[] args)
        {
            var button = new Button();
            button.Click();
            Console.ReadKey();
        }
    }

    public class Button
    {
        public void Click()
        {
            DoWork(1);
        }

        private void DoWork(int times)
        {   
            string timesText = "time(s)";
            if(times == 1)
            {
                timesText = "time";
            }
            Console.WriteLine($"Do Work {times} {timesText}!");
        }
    }

}
