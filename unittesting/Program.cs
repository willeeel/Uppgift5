using System;
using Microsoft.Azure.Devices.Client;
using System.Text;
using System.Threading.Tasks;
using SharedLibraries.Services;
using Newtonsoft.Json;

namespace ConsoleApp
{
    class Program
    {
        private static readonly string _conn = "HostName=william-iothub.azure-devices.net;DeviceId=consoleapp;SharedAccessKey=NSMvREJlr/jxQQa+T/sKS8xGlDvDJsuLkgh1FXmcOeE=";
        private static readonly DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(_conn, TransportType.Mqtt);
        private static int telemetryInterval = 5;
        private static Random rnd = new Random();
        static void Main(string[] args)
        {
            deviceClient.SetMethodHandlerAsync("SetTelemetryInterval", SetTelemetryInterval, null).Wait();
            SendMessageAsync().GetAwaiter();

            Console.ReadKey();
        }
        private static Task<MethodResponse> SetTelemetryInterval(MethodRequest request, object userContext)
        {
            var payload = Encoding.UTF8.GetString(request.Data).Replace("\"", "");

            if (Int32.TryParse(payload, out telemetryInterval))
            {
                Console.WriteLine($"Interval has been set to: {telemetryInterval} seconds.");
                string json = "{\"result\": \"Executed direct method: " + request.Name + "\"}";
                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(json), 200));
            }
            else
            {
                string json = "{\"result\": \"Method not implemented\"}";
                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(json), 501));
            }
        }
        public static async Task SendMessageAsync()
        {
            while (true)
            {
                int temp = rnd.Next(10, 50);
                int hum = rnd.Next(30, 60);

                var data = new
                {
                    temperature = temp,
                    humidity = hum
                };

                var json = JsonConvert.SerializeObject(data);
                var payload = new Message(Encoding.UTF8.GetBytes(json));
                payload.Properties.Add("temperatureAlert", (temp > 30) ? "true" : "false");

                await deviceClient.SendEventAsync(payload);
                Console.WriteLine($"Message sent: {json}");

                await Task.Delay(telemetryInterval * 1000);
            }
        }
    }
}
