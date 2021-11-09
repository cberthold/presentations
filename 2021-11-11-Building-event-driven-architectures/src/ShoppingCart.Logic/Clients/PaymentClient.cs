using ShoppingCart.Logic.Payment;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShoppingCart.Logic.Clients
{
    public class PaymentClient
    {
        public PaymentClient(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public async Task<AuthorizationResponse> AuthorizePayment(AuthorizationRequest request)
        {
            // Serialize our concrete class into a JSON String
            var stringPayload = JsonSerializer.Serialize(request);

            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            var response = await Client.PostAsync("Process", httpContent);

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException("Unhandled exception occurred processing credit card");

            var stream = await response.Content.ReadAsStreamAsync();
            var authResponse = await JsonSerializer.DeserializeAsync<AuthorizationResponse>(stream);

            return authResponse;
        }
    }
}
