using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client2
{
    class Program
    {

      static void Main()
      {
        MainAsync().Wait();
        // or, if you want to avoid exceptions being wrapped into AggregateException:
        //  MainAsync().GetAwaiter().GetResult();
      }

      private static async Task MainAsync()
      {
        {
          Console.WriteLine("Hello World!");
          Console.ReadKey();

          var client = new HttpClient();

          var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5000");
          if (disco.IsError)
          {
            Console.WriteLine(disco.Error);
            return;
          }
        // request token
        var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
          {
            Address = disco.TokenEndpoint,
            ClientId = "ro.client",
            ClientSecret = "secret",

            UserName = "alice",
            Password = "password2",
            Scope = "api1"
          });

          if (tokenResponse.IsError)
          {
            Console.WriteLine(tokenResponse.Error);
            return;
          }

          Console.WriteLine(tokenResponse.Json);
          Console.WriteLine("\n\n");

          // call api
          var apiClient = new HttpClient();
          apiClient.SetBearerToken(tokenResponse.AccessToken);

          var response = await apiClient.GetAsync("http://localhost:5001/api/identity");
          if (!response.IsSuccessStatusCode)
          {
            Console.WriteLine(response.StatusCode);
          }
          else
          {
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(JArray.Parse(content));
          }

          Console.ReadKey();
        Console.ReadKey();
        }
      }
    }
}
