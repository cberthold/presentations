// See https://aka.ms/new-console-template for more information
using System.Net.Http.Json;
using Newtonsoft.Json;


while(true)
{
    Console.WriteLine("Hit any key to start. ESC to exit.");
    var keyInfo = Console.ReadKey();

    if(keyInfo.Key == ConsoleKey.Escape)
    {
        break;
    }


    Console.WriteLine("Executing Say Hello!");

    var client = new HttpClient();
    client.BaseAddress = new Uri("http://localhost:7029/api/");

    // execute SayHelloOrchestration_HttpStart
    var response = await client.GetAsync("SayHelloOrchestration_HttpStart");


    HashSet<string> completedStates = new HashSet<string>()
    {
        "Completed",
        "Failed",
    };

    // we should get accepted
    if(response.StatusCode == System.Net.HttpStatusCode.Accepted)
    {
        // get the json object from our response
        string json = await response.Content.ReadAsStringAsync();
        dynamic result = JsonConvert.DeserializeObject(json);
        string id = result.id;
        string uri = result.statusQueryGetUri;
        Console.WriteLine($"Orchestration started with instances id: {id}");

        while(true)
        {
            await Task.Delay(100);

            response = await client.GetAsync(uri);

            json = await response.Content.ReadAsStringAsync();
            result = JsonConvert.DeserializeObject(json);

            string name = result.name;
            string instanceId = result.instanceId;
            string runtimeStatus = result.runtimeStatus;

            if(response.StatusCode == System.Net.HttpStatusCode.OK && completedStates.Contains(runtimeStatus))
            {
                var output = result.output;
                Console.WriteLine($"Orchestration: {name} with instanceId: {instanceId} completed with status: {runtimeStatus}");
                Console.WriteLine($"Output: {output}");
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
