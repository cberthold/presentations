using static Shared.Demo3.Toppings.Meats;
using static Shared.Demo3.Toppings.Veggies;
using static Shared.Demo3.Toppings;
using System.Text.Json.Serialization;

namespace Shared.Demo3
{
    public class OrderCounter
    {
        private static object _lock = new object();
        public static int Number { get; private set; }

        public static int GetNext()
        {
            lock (_lock)
            {
                int num = Number++;
                return num;
            }
        }
    }

    public class PizzaOrderRequest
    {
        public string InstanceId { get; set; }
        public PizzaOrder Order { get; set; }
    }

    public class PizzaOrder
    {
        public int OrderNumber { get; set; }

        public List<Veggies> Veggies { get; set; } = new List<Veggies>();

        public List<Meats> Meats { get; set; } = new List<Meats>();

        public static PizzaOrder Hawaiian()
        {
            var order = new PizzaOrder();
            order.Meats.Add(Ham);
            order.Meats.Add(Bacon);
            order.Veggies.Add(Pineapple);
            return order;
        }

        public static PizzaOrder PepperoniPizza()
        {
            var order = new PizzaOrder();
            order.Meats.Add(Pepperoni);
            return order;
        }

        public static PizzaOrder SteakAndCheese()
        {
            var order = new PizzaOrder();
            order.Meats.Add(Steak);
            order.Veggies.Add(GreenPeppers);
            order.Veggies.Add(Onions);
            order.Veggies.Add(Mushrooms);
            return order;
        }
    }

    public class Toppings
    {

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum Veggies
        {
            None = 0,
            GreenPeppers = 1,
            Onions = 2,
            Pineapple = 3,
            Mushrooms = 4,

        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum Meats
        {
            None = 0,
            Pepperoni = 1,
            Sausage = 2,
            Ham = 3,
            Steak = 4,
            Bacon = 5,
        }
    }
}
