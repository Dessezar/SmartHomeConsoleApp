
using Microsoft.Azure.Devices;

namespace Shared.Handlers;

public class IotHubHandler(string connectionString)
{
    private readonly RegistryManager? _registry = RegistryManager.CreateFromConnectionString(connectionString);
    private readonly ServiceClient? _serviceClient = ServiceClient.CreateFromConnectionString(connectionString);
}
