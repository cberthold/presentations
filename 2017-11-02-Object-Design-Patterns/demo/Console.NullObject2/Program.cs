using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolApp.NullObject2
{
    class Program
    {
        public INewFeatureCommand NewFeature { get; set; } = new NullFeatureCommand();
        public bool IsNewFeatureEnabled { get; set; }

        public void Run()
        {
            if(IsNewFeatureEnabled)
            {
                NewFeature = new NewFeatureCommand();
            }

            Console.WriteLine("Existing feature step 1");

            NewFeature.Execute();

            Console.WriteLine("Existing feature step 2");
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            var program = new Program();
            program.IsNewFeatureEnabled = true;
            program.Run();
        }
    }

    public interface INewFeatureCommand
    {
        void Execute();
    }

    public class NullFeatureCommand : INewFeatureCommand
    {
        public void Execute()
        {
            // DONT DO ANYTHING
        }
    }

    public class NewFeatureCommand : INewFeatureCommand
    {
        public void Execute()
        {
            Console.WriteLine("New feature added");
        }
    }
}
