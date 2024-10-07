using Microsoft.Azure.Devices.Client;
using Shared.Models;

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
}
