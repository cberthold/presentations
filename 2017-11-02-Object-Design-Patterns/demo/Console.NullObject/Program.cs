using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolApp.NullObject
{
    class Program
    {
        public INewFeatureCommand NewFeature { get; set; }

        public void Run()
        {
            
            Console.WriteLine("Existing feature step 1");

            NewFeature.Execute();

            Console.WriteLine("Existing feature step 2");
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            var program = new Program();
            program.Run();
        }
    }

    public interface INewFeatureCommand
    {
        void Execute();
    }

    public class NewFeatureCommand : INewFeatureCommand
    {
        public void Execute()
        {
            Console.WriteLine("New feature added");
        }
    }
}
