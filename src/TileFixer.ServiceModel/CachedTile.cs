namespace TileFixer.ServiceModel
{
  public class CachedTile
  {
    public byte[] Image { get; set; }
    public TileBoundingBox Bounds { get; set; }
  }
}