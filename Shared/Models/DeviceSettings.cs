namespace Shared.Models;

public class DeviceSettings
{
    public string DeviceId { get; set; } = null!;
    public string? DeviceName { get; set;}
    public string? DeviceType { get; set; }
    public bool DeviceState { get; set; }

    public bool ConnectionsState { get; set; }
    public string ConnectionString { get; set; } = null!;
}
