namespace gRPC.Server.Api.Connections;

/// <summary>
/// Provides data that can be used for the identification of a gRPC client connection
/// </summary>
/// <param name="connectionId">The gRPC client connection ID.</param>
public class ConnectionKey(string connectionId) : IEquatable<ConnectionKey>
{
  public string ConnectionId { get; set; } = connectionId;

  public bool Equals(ConnectionKey? other)
  {
    if (other is null) return false;
    return ConnectionId == other.ConnectionId;
  }

  public override bool Equals(object? obj)
  {
    return Equals(obj as ConnectionKey);
  }

  public override int GetHashCode()
  {
    return ConnectionId.GetHashCode();
  }
}