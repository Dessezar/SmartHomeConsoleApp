using Shared.Handlers;

var deviceClientHandler = new DeviceClientHandler("", "", "");
var result = await deviceClientHandler.InitializeAsync();

Console.WriteLine(result.Message);



