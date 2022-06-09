using System;

namespace InterfacesAndClasses
{

    public interface IRun { void Run(); }
    public interface ISwim { void Swim(); }
    public interface ITalk { void Talk(); }

    public class Person : IRun, ISwim, ITalk
    {
        public void Run()
        {
            Console.WriteLine("Person Runs with Two Legs");
        }

        public void Swim()
        {
            Console.WriteLine("Person Swims using the Breaststroke");
        }

        public void Talk()
        {
            Console.WriteLine("Person Talks with their Mouth");
        }
    }

    public class Shark : ISwim
    {
        public void Swim()
        {
            Console.WriteLine("Shark Swims using fins and tail");
        }
    }


    public class Dog : IRun, ISwim
    {
        public void Run()
        {
            Console.WriteLine("Dog Runs with Four Legs");
        }

        public void Swim()
        {
            Console.WriteLine("Dog Swims using doggy paddle");
        }
    }




    class Program
    {
        static void Main(string[] args)
        {
            var person = new Person();
            var dog = new Dog();
            var shark = new Shark();

            Console.WriteLine("---- Interface Is-A -----");
            MakeThemRunSwimAndOrTalk(person);
            MakeThemRunSwimAndOrTalk(dog);
            MakeThemRunSwimAndOrTalk(shark);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();


            // example of swapping interfaces at runtime

            Console.WriteLine("---- Swapping Implementations At Runtime -----");
            ISwim iswim = person;
            iswim.Swim();
            
            iswim = dog;
            iswim.Swim();

            iswim = shark;
            iswim.Swim();
        }

        private static void MakeThemRunSwimAndOrTalk(object o)
        {
            string className = o.GetType().Name;

            Console.WriteLine($"Going to make the {className} Run Swim And/Or Talk");

            if(o is IRun iCanRunThen)
            {
                Console.WriteLine($"Going to make the {className} Run since they can run");
                iCanRunThen.Run();
            }
            else
            {
                Console.WriteLine($"{className} doesn't know how to Run");
            }

            if (o is ISwim iCanSwimThen)
            {
                Console.WriteLine($"Going to make the {className} Swim since they can swim");
                iCanSwimThen.Swim();
            }
            else
            {
                Console.WriteLine($"{className} doesn't know how to Swim");
            }

            if (o is ITalk iCanTalkThen)
            {
                Console.WriteLine($"Going to make the {className} Talk since they can talk");
                iCanTalkThen.Talk();
            }
            else
            {
                Console.WriteLine($"{className} doesn't know how to Talk");
            }
        }
    }
}
