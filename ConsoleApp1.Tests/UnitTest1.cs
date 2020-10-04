using System;
using Xunit;
using Microsoft.Azure.Devices.Client;
using SharedLibraries.Services;

namespace ConsoleApp1.Tests
{
    public class UnitTest1
    {
        [Theory]
        [InlineData("consoleapp", "SetTelemetryInterval", "5", "200")]
        [InlineData("consoleapp", "GetInterval", "5", "501")]
        public void Test1(string targetDevice, string MethodName, string payload, string expected)
        {
            var service = new ServiceClientService("HostName=william-iothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=7oR1jLMwmBf2gde6gIbZBh//jRFBlaZ1ea+O3L3zqiA=");
            var response = service.InvokeMethodAsync(targetDevice, MethodName, payload);
            Assert.Equal(expected, response.Result.Status.ToString());
        }
    }
}
