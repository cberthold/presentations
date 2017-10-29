using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Adapter2
{
    class Program
    {
        static void Main(string[] args)
        {
            var sayHello = new SayHelloClientV2();
            sayHello.Execute(Language.Spanish);
            Console.ReadKey();
        }
    }

    public enum Language
    {
        English,
        Spanish
    }


    public class SayHelloClientV2
    {
        public void Execute(Language language)
        {
            if(language == Language.English)
            {
                var english = GetEnglishHelloService();
                var hello = english.SayHello();

                Console.WriteLine(hello.Text);
            }
            else if(language == Language.Spanish)
            {
                var spanish = GetSpanishHelloService();
                var hello = spanish.SayHola();

                Console.WriteLine(hello.Texto);
            }
            else
            {
                throw new NotImplementedException();
            }
            
        }

        private EnglishHelloService GetEnglishHelloService()
        {
            return new EnglishHelloService();
        }

        private SpanishHelloService GetSpanishHelloService()
        {
            return new SpanishHelloService();
        }

    }

    #region Spanish Service Impl

    public class SpanishHelloService
    {
        public SpanishResponse SayHola()
        {
            return new SpanishResponse()
            {
                Texto = "Hola",
            };
        }
    }

    public class SpanishResponse
    {
        public string Texto { get; set; }
    }

    #endregion

    #region English Service Impl

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

    #endregion

}
