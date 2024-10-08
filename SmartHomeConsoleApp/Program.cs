using Shared.Handlers;





var deviceClientHandler = new DeviceClientHandler("86f55ef4-bb5c-4b9f-9fe2-b4ad27599b5d", "console app", "console");
var result = await deviceClientHandler.InitializeAsync();

Console.WriteLine(result.Message);



