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
            sayHello.Execute(Language.English);
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
            ILanguageServiceAdapter adapter = null;

            if (language == Language.English)
            {
                adapter = new EnglishLanguageServiceAdapter(GetEnglishHelloService());
            }
            else if (language == Language.Spanish)
            {
                adapter = new SpanishLanguageServiceAdapter(GetSpanishHelloService());
            }
            else
            {
                throw new NotImplementedException();
            }

            var response = adapter.SayHello();
            Console.WriteLine(response.Text);
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

    public class LanguageResponse
    {
        public string Text { get; set; }
    }

    public interface ILanguageServiceAdapter
    {
        LanguageResponse SayHello();
    }

    public class EnglishLanguageServiceAdapter :
        ILanguageServiceAdapter
    {
        private EnglishHelloService englishHelloService;

        public EnglishLanguageServiceAdapter(EnglishHelloService englishHelloService)
        {
            this.englishHelloService = englishHelloService;
        }

        public LanguageResponse SayHello()
        {
            var english = englishHelloService.SayHello();
            return new LanguageResponse
            {
                Text = english.Text,
            };
        }
    }

    public class SpanishLanguageServiceAdapter :
        ILanguageServiceAdapter
    {
        readonly SpanishHelloService service;

        public SpanishLanguageServiceAdapter(SpanishHelloService service)
        {
            this.service = service;
        }


        public LanguageResponse SayHello()
        {
            var spanish = service.SayHola();

            var response = new LanguageResponse()
            {
                Text = spanish.Texto,
            };

            return response;
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
