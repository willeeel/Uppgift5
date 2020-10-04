using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Devices.Client;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SharedLibraries.Services
{
    public class DeviceClientService
    {

        public static DeviceClient deviceClient;
        private int _interval = 10;
        private static int telemetryInterval = 5;
        private static Random rnd = new Random();
        public DeviceClientService(string connectionstring)
        {
            deviceClient = DeviceClient.CreateFromConnectionString(connectionstring, TransportType.Mqtt);
            deviceClient.SetMethodHandlerAsync("SetInterval", SetTelemetryIntervalAsync, null).GetAwaiter();
        }
        public async Task<MethodResponse> SetTelemetryIntervalAsync(MethodRequest methodRequest, object userContext)
        {
            SetInterval(Convert.ToInt32(methodRequest.DataAsJson));
            return await Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(methodRequest.DataAsJson), 200));
        }
        public bool SetInterval(int interval)
        {
            try
            {
                _interval = telemetryInterval;
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
