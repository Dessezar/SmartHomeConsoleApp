using Shared.Handlers;
using Shared.Models;

namespace MainApp.ViewModels;

public class HomeViewModel
{
    private readonly IotHubHandler _iotHub;
    public Timer? Timer { get; set; }
    public int TimerInterval { get; set; } = 4000;

    public HomeViewModel(IotHubHandler iotHub)
    {
        _iotHub = iotHub;
    }

    public async Task<IEnumerable<DeviceSettings>> GetDevicesAsync()
    {
        return _iotHub.GetDevicesAsync();
    }

    public async Task OnDeviceStateChanged(DeviceSettings device)
    {
        Timer?.Change(Timeout.Infinite, Timeout.Infinite);
        await SendDirectMethodAsync(device);
        Timer?.Change(TimerInterval, TimerInterval);
    }
    public async Task SendDirectMethodAsync(DeviceSettings device)
    {
        var methodName = device.DeviceState ? "stop" : "start";
        await _iotHub.SendDirectMethodAsync(device.DeviceId, methodName);
    }
}
