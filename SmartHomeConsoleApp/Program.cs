using Microsoft.Azure.Devices;
using Shared.Handlers;





var dc = new DeviceClientHandler("86f55ef4-bb5c-4b9f-9fe2-b4ad27599b5d", "console", "console app");
var initializeResult = dc.Initialize();

Console.WriteLine(initializeResult.Message);

dc.Settings.DeviceStateChanged += (deviceState) =>
{
    Console.WriteLine($"Device state changed to {deviceState}");
};


Console.WriteLine("Press any key to close application.");

AppDomain.CurrentDomain.ProcessExit += (s, e) =>
{


    var disconnectResult = dc.Disconnect();
    Console.WriteLine(disconnectResult.Message);
};


Console.ReadKey();


