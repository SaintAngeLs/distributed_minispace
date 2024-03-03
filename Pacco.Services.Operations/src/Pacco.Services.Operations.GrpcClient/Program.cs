using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Services.Operations;

namespace Pacco.Services.Operations.GrpcClient
{
    class Program
    {
        private static GrpcOperationsService.GrpcOperationsServiceClient _client;

        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented
        };

        private static readonly IDictionary<string, Func<Task>> Actions = new Dictionary<string, Func<Task>>
        {
            ["1"] = GetOperationAsync,
            ["2"] = SubscribeOperationsStreamAsync,
        };

        static async Task Main(string[] args)
        {
            var address = GetAddress(args);
            
            // Only for the local development purposes.
            var httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            var httpClient = new HttpClient(httpClientHandler);
            
            var channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
            {
                HttpClient = httpClient
            });
            _client = new GrpcOperationsService.GrpcOperationsServiceClient(channel);
            Console.WriteLine($"Created a GRPC client for an address: '{address}'");
            await InitAsync();
        }

        private static string GetAddress(string[] args)
        {
            var host = string.Empty;
            var port = 0;

            if (args?.Any() == true && args.Length >= 2)
            {
                host = args[0];
                if (int.TryParse(args[1], out var providedPort))
                {
                    port = providedPort;
                }
            }

            if (string.IsNullOrWhiteSpace(host))
            {
                host = "localhost";
            }

            if (port <= 0)
            {
                port = 50050;
            }
            
            return $"https://{host}:{port}";
        }

        private static async Task InitAsync()
        {
            const string message = "\nOptions (1-2):" +
                                   "\n1. Get the single operation by id" +
                                   "\n2. Subscribe to the operations stream" +
                                   "\nType 'q' to quit.\n";

            var option = string.Empty;
            while (option != "q")
            {
                Console.WriteLine(message);
                Console.Write("> ");
                option = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(option))
                {
                    Console.WriteLine("Missing option");
                    continue;
                }

                Console.WriteLine();
                if (Actions.ContainsKey(option))
                {
                    try
                    {
                        await Actions[option]();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }

                    continue;
                }

                Console.WriteLine($"Invalid option: {option}");
            }
        }

        private static async Task GetOperationAsync()
        {
            Console.Write("Type the operation id: ");
            var id = Console.ReadLine();
            Console.WriteLine("Sending the request...");
            var response = await _client.GetOperationAsync(new GetOperationRequest
            {
                Id = id
            });
            if (string.IsNullOrWhiteSpace(response.Id))
            {
                Console.WriteLine($"* Operation was not found for id: {id} *");
                return;
            }

            Console.WriteLine($"* Operation was found for id: {id} *");
            DisplayOperation(response);
        }

        private static async Task SubscribeOperationsStreamAsync()
        {
            Console.WriteLine("Subscribing to the operations stream...");
            using (var stream = _client.SubscribeOperations(new Empty()))
            {
                while (await stream.ResponseStream.MoveNext())
                {
                    Console.WriteLine("* Received the data from the operations stream *");
                    DisplayOperation(stream.ResponseStream.Current);
                }
            }
        }

        private static void DisplayOperation(GetOperationResponse response)
            => Console.WriteLine(JsonConvert.SerializeObject(response, JsonSerializerSettings));
    }
}
