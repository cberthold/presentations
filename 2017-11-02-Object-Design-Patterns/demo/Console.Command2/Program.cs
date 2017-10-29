using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Command2
{
    class Program
    {
        static void Main(string[] args)
        {
            var button = new Button();
            button.Command = new DoWorkCommand()
            {
                Times = 2,
            };
            button.Click();
            Console.ReadKey();
        }
    }

    public interface ICommand
    {
        void Execute();
    }

    public class DoWorkCommand : ICommand
    {
        public int Times { get; set; }

        public void Execute()
        {
            DoWork(Times);
        }

        private void DoWork(int times)
        {
            string timesText = "time(s)";
            if (times == 1)
            {
                timesText = "time";
            }
            Console.WriteLine($"Do Work {times} {timesText}!");
        }
    }


    public class Button
    {
        public ICommand Command { get; set; }

        public void Click()
        {
            Command.Execute();
        }


    }

}
