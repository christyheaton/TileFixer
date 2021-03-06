using ServiceStack.Configuration;

namespace Tile.Fixer
{
  public class RedisConfig : Config
  {
    public string Host { get; set; }
    public int Port { get; set; }
    public int Database { get; set; }
    public int Timeout { get; set; }
  }
}