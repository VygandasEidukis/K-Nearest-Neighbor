namespace K_nearest_neighbors.Models
{
    public interface IMesurable
    {
        float? Distance { get; set; }
        float X { get; set; }
        float Y { get; set; }
    }
}
