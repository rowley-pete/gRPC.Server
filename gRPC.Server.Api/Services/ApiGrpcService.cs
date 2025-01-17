using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace gRPC.Server.Api.Services;

public class ApiGrpcService(ILogger<ApiGrpcService> logger) : ApiService.ApiServiceBase
{
  private readonly ILogger<ApiGrpcService> _logger = logger;

  public override Task<VersionResponse> GetVersion(Empty request, ServerCallContext context)
  {
    _logger.LogInformation("Getting version");

    var response = new VersionResponse
    {
      Version = "0.0.1"
    };

    return Task.FromResult(response);
  }
}