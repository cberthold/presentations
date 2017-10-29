using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp.Decorator2
{
    class Program
    {
        static void Main(string[] args)
        {
            var button = new Button();
            button.Command = CreateCommand<DoWorkCommand>(true);
            button.Click();
            Console.ReadKey();
        }

        private static ICommand CreateCommand<TCommand>(bool enableLogging)
            where TCommand : ICommand, new()
        {
            ICommand command = new TCommand();

            if(enableLogging)
            {
                command = new CommandLoggingDecorator(command);
            }

            return command;
        }

        public class CommandLoggingDecorator : ICommand
        {
            public ICommand decoratedCommand;
            public CommandLoggingDecorator(ICommand decoratedCommand)
            {
                this.decoratedCommand = decoratedCommand;
            }

            public void Execute()
            {
                var commandType = decoratedCommand.GetType();
                var commandName = commandType.Name;
                Console.WriteLine($"Began {commandName} at {DateTime.Now.ToLongTimeString()}");

                decoratedCommand.Execute();

                // creating new thread sleep bug for future performance
                Thread.Sleep(3000);

                Console.WriteLine($"Ended {commandName} at {DateTime.Now.ToLongTimeString()}");
            }
        }

    }
    

    public interface ICommand
    {
        void Execute();
    }

    #region Original Command Implementation

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

    #endregion

}
