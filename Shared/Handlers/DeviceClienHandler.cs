using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using Shared.Models;
using System.CodeDom.Compiler;
using System.Text;

namespace Shared.Handlers;

public class DeviceClienHandler
{
    private readonly DeviceSettings _settings = new();
    private DeviceClient? _client;
    public DeviceClienHandler(string deviceId, string deviceType, string deviceName)
    {
        _settings!.DeviceId = deviceId;
        _settings.DeviceType = deviceType;
        _settings.DeviceName = deviceName;
    }

    public void Initialize()
    {
        _client = DeviceClient.CreateFromConnectionString(_settings.ConnectionString);
    }

    public Task<MethodResponse> DirectMethodDefaultCallback(MethodRequest request, object useContext)
    {
        string method = request.Name.ToLower();

        switch (request.Name.ToLower())
        {
            case "start":
                return Task.FromResult( OnStart());

            case "stop":
                return OnStop();

            default:
                break;
        }
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
