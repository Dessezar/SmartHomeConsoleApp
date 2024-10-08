using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using Shared.Models;
using System.CodeDom.Compiler;
using System.Text;

namespace Shared.Handlers;

public class DeviceClienHandler
{
    private readonly DeviceSettings _settings = new();
    public DeviceClient? _client {  get; private set; }
    public DeviceClienHandler(string deviceId, string deviceType, string deviceName)
    {
        _settings!.DeviceId = deviceId;
        _settings.DeviceType = deviceType;
        _settings.DeviceName = deviceName;
    }

    public void Initialize()
    {
        _client = DeviceClient.CreateFromConnectionString(_settings.ConnectionString);
        _client.SetMethodDefaultHandlerAsync(DirectMethodDefaultCallback, null);
    }

    public Task<MethodResponse> DirectMethodDefaultCallback(MethodRequest request, object useContext)
    {
        var methodResponse = request.Name.ToLower() switch
        {
            "start" => OnStart(),
            "stop" => OnStop(),
            _ => GenerateMethodResponse("No suitble method found", 404),
        };

        return Task.FromResult(methodResponse);

        //string method = request.Name.ToLower();
        //switch (request.Name.ToLower())
        //{
        //    case "start":
        //        return Task.FromResult(OnStart());
        //    case "stop":
        //        return OnStop();
        //    default:
        //        break;
        //}
    }

    public MethodResponse OnStart()
    {
        _settings.DeviceState = true;
        return GenerateMethodResponse("DeviceState changed set to start", 200);
    }

    public MethodResponse OnStop()
    {
        _settings.DeviceState = false;
        return GenerateMethodResponse("DeviceState changed set to stop", 200);

    }

    public MethodResponse GenerateMethodResponse(string message, int statusCode)
    {
        try
        {
            var json = JsonConvert.SerializeObject(new { Message = message });
            var methodResponse = new MethodResponse(Encoding.UTF8.GetBytes(json), statusCode);
            return methodResponse;

        }
        catch(Exception ex) 
        {
            var json = JsonConvert.SerializeObject(new { Message = ex.Message });
            var methodResponse = new MethodResponse(Encoding.UTF8.GetBytes(json), statusCode);
            return methodResponse;
        }

    }
}
