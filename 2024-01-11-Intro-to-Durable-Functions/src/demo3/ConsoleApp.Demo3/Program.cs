// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Shared.Demo3;


while (true)
{
    Console.WriteLine("Buy a pizza. ESC to exit.");
    Console.WriteLine("1: Hawaiian");
    Console.WriteLine("2: Pepperoni");
    Console.WriteLine("3: Steak and Cheese");

    var keyInfo = Console.ReadKey();

    PizzaOrder order = default;

    if (keyInfo.Key == ConsoleKey.Escape)
    {
        break;
    }
    else if (keyInfo.KeyChar == '1')
    {
        order = PizzaOrder.Hawaiian();
    }
    else if (keyInfo.KeyChar == '2')
    {
        order = PizzaOrder.PepperoniPizza();
    }
    else if (keyInfo.KeyChar == '3')
    {
        order = PizzaOrder.SteakAndCheese();
    }
    else
    {
        continue;
    }

    Stopwatch sw = Stopwatch.StartNew();
    Console.WriteLine("Executing Pizza Order!");

    var client = new HttpClient();
    client.BaseAddress = new Uri("http://localhost:7034/api/");

    // execute SayHelloOrchestration_HttpStart
    var content = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");
    var response = await client.PostAsync("OrderPizzaOrchestration_HttpStart", content);

    HashSet<string> completedStates = new HashSet<string>()
    {
        "Completed",
        "Failed",
    };

    // we should get accepted
    if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
    {
        // get the json object from our response
        string json = await response.Content.ReadAsStringAsync();
        dynamic result = JsonConvert.DeserializeObject(json);
        string id = result.id;
        string uri = result.statusQueryGetUri;
        Console.WriteLine($"Orchestration started with instances id: {id}");

        while (true)
        {
            await Task.Delay(100);

            response = await client.GetAsync(uri);

            json = await response.Content.ReadAsStringAsync();
            result = JsonConvert.DeserializeObject(json);

            string name = result.name;
            string instanceId = result.instanceId;
            string runtimeStatus = result.runtimeStatus;

            if (response.StatusCode == System.Net.HttpStatusCode.OK && completedStates.Contains(runtimeStatus))
            {
                var output = result.output;
                Console.WriteLine($"Orchestration: {name} with instanceId: {instanceId} completed with status: {runtimeStatus}");
                Console.WriteLine($"Output: {output}");
                sw.Stop();
                Console.WriteLine($"Took {sw.ElapsedMilliseconds} milliseconds");

                break;
            }
            else
            {
                Console.WriteLine($"Orchestration: {name} with instanceId: {instanceId} is: {runtimeStatus}");
            }


        }
    }
    else
    {
        // OK, Error, etc.
        Console.WriteLine("Demo conidition not supported - oops how did we get here?");
    }
}
