﻿using Microsoft.Azure.Devices;
using Shared.Handlers;





var dc = new DeviceClientHandler("86f55ef4-bb5c-4b9f-9fe2-b4ad27599b5d", "console", "console app");
var result = dc.Initialize();

Console.WriteLine(result.Message);

dc.Settings.DeviceStateChanged += (deviceState) =>
{
    Console.WriteLine($"Device state changed to {deviceState}");
};

AppDomain.CreateDomain.ProcessExit += async (s, e) =>
{
    var result = await dc.DisconnectAsync();
    Console.WriteLine(result.Message);

}

Console.WriteLine("Press any key to coles application.");
Console.ReadKey();  



