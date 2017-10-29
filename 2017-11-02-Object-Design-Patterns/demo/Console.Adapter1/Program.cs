using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Adapter1
{
    class Program
    {
        static void Main(string[] args)
        {
            var sayHello = new SayHelloClient();
            sayHello.Execute();
            Console.ReadKey();
        }
    }
    

    public class SayHelloClient
    {
        public void Execute()
        {
            var service = GetHelloService();
            var hello = service.SayHello();

            Console.WriteLine(hello.Text);
        }

        private EnglishHelloService GetHelloService()
        {
            return new EnglishHelloService();
        }
        
    }
    
    public class EnglishHelloService
    {
        public EnglishResponse SayHello()
        {
            return new EnglishResponse()
            {
                Text = "Hello",
            };
        }
    }

    public class EnglishResponse
    {
        public string Text { get; set; }
    }
    
}
