using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using Shared.Models;
using System.CodeDom.Compiler;
using System.Text;

namespace Shared.Handlers;

public class DeviceClientHandler
{
    public DeviceSettings Settings { get; private set; } = new();
    private DeviceClient? _client;
    public DeviceClientHandler(string deviceId, string deviceType, string deviceName)
    {
        Settings!.DeviceId = deviceId;
        Settings.DeviceType = deviceType;
        Settings.DeviceName = deviceName;
        Settings.ConnectionString = "HostName=systemutvecklingIotHub.azure-devices.net;DeviceId=86f55ef4-bb5c-4b9f-9fe2-b4ad27599b5d;SharedAccessKey=XiE8GjOXmNV2xTzSlggs4QhEaDD+mzlgrgIO6C8D1Rs=";
    }

    public ResultResponse Initialize()
    {
        var response = new ResultResponse();
        try
        {
            _client = DeviceClient.CreateFromConnectionString(Settings.ConnectionString);
            if (_client != null)
            {
                Task.WhenAll(_client.SetMethodDefaultHandlerAsync(DirectMethodDefaultCallback, null), UpdateDeviceTwinDevicePropertiesAsync());


                response.Succeeded = true;
                response.Message = "device initalized";
            }
            else
            {
                response.Succeeded = false;
                response.Message = "No device client was found";
            }
        }
        catch (Exception ex)
        {
            response.Succeeded = false;
            response.Message = $"{ex.Message}";
        }
        return response;
    }

    public async Task<MethodResponse> DirectMethodDefaultCallback(MethodRequest request, object useContext)
    {
        var methodResponse = request.Name.ToLower() switch
        {
            "start" => await OnStartAsync(),
            "stop" => await OnStopAsync(),
            _ => GenerateMethodResponse("No suitble method found", 404),
        };

        return methodResponse;

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

    public async Task<MethodResponse> OnStartAsync()
    {
        Settings.DeviceState = true;
        var result = await UpdateDeviceTwinDeviceStateAsync();
        if (result.Succeeded)
        {
            return GenerateMethodResponse("DeviceState changed set to start", 200);
        }
        else
        {
            return GenerateMethodResponse($"{result.Message}", 400);
        }
    }

    public async Task<MethodResponse> OnStopAsync()
    {
        Settings.DeviceState = false;

        var result = await UpdateDeviceTwinDeviceStateAsync();

        if (result.Succeeded)
        {
            return GenerateMethodResponse("DeviceState changed set to stop", 200);
        }
        else
        {
            return GenerateMethodResponse($"{result.Message}", 400);

        }
    }

    public MethodResponse GenerateMethodResponse(string message, int statusCode)
    {
        try
        {
            var json = JsonConvert.SerializeObject(new { Message = message });
            var methodResponse = new MethodResponse(Encoding.UTF8.GetBytes(json), statusCode);
            return methodResponse;

        }
        catch (Exception ex)
        {
            var json = JsonConvert.SerializeObject(new { Message = ex.Message });
            var methodResponse = new MethodResponse(Encoding.UTF8.GetBytes(json), statusCode);
            return methodResponse;
        }
    }

    public async Task<ResultResponse> UpdateDeviceTwinDeviceStateAsync()
    {
        var response = new ResultResponse();
        try
        {
            var reportedProperties = new TwinCollection
            {
                ["deviceState"] = Settings.DeviceState,
            };

            if (_client != null)
            {
                await _client!.UpdateReportedPropertiesAsync(reportedProperties);
                response.Succeeded = true;
            }
            else
            {
                response.Succeeded = false;
                response.Message = "No device client was found";
            }
        }
        catch (Exception ex)
        {
            response.Succeeded = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ResultResponse> UpdateDeviceTwinDevicePropertiesAsync()
    {
        var response = new ResultResponse();
        try
        {
            var reportedProperties = new TwinCollection
            {
                ["connectionState"] = Settings.ConnectionsState,
                ["deviceName"] = Settings.DeviceName,
                ["deviceType"] = Settings.DeviceType,
                ["deviceState"] = Settings.DeviceState,
            };

            if (_client != null)
            {
                await _client!.UpdateReportedPropertiesAsync(reportedProperties);
                response.Succeeded = true;
            }
            else
            {
                response.Succeeded = false;
                response.Message = "No device client was found";
            }
        }
        catch (Exception ex)
        {
            response.Succeeded = false;
            response.Message = ex.Message;
        }
        return response;
    }
}
