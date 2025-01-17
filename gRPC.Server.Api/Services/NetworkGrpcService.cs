using System.Collections.Concurrent;
using Google.Protobuf.WellKnownTypes;
using gRPC.Server.Api.Connections;
using Grpc.Core;

namespace gRPC.Server.Api.Services;

public class NetworkGrpcService(ILogger<NetworkGrpcService> logger) : NetworkService.NetworkServiceBase
{
  private readonly static ConcurrentDictionary<ConnectionKey, IServerStreamWriter<ConnectedNetwork>> _networkWriters = [];

  private readonly ILogger<NetworkGrpcService> _logger = logger;

  public override async Task SubscribeNetwork(Empty request, IServerStreamWriter<ConnectedNetwork> responseStream, ServerCallContext context)
  {
    var connectionKey = new ConnectionKey(context.GetHttpContext().Connection.Id);

    if (!_networkWriters.TryAdd(connectionKey, responseStream))
    {
      return;
    }

    _logger.LogInformation($"Added network observer for connection ID: {connectionKey.ConnectionId}");

    while (!context.CancellationToken.IsCancellationRequested)
    {
      await Task.Delay(100);
    }

    _networkWriters.TryRemove(connectionKey, out _);
    _logger.LogInformation($"Removed network observer for connection ID: {connectionKey.ConnectionId}");
  }

  public override async Task<Empty> PushNetworkInterface(ConnectedNetwork request, ServerCallContext context)
  {
    _logger.LogInformation($"Received a network interface update. {request.Name}");
    foreach (var observer in _networkWriters)
    {
      try
      {
        _logger.LogInformation($"Sending to network observer with ID: {observer.Key}");
        await observer.Value.WriteAsync(request);
      }
      catch
      {
        _logger.LogInformation($"Sending to network observer with ID: {observer.Key} failed for {request.Name}, removing from observers list");
        _networkWriters.TryRemove(observer.Key, out _);
      }
    }

    return new Empty();
  }
}