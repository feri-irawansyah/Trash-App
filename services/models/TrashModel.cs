namespace Services.Models;
public class Trash
{
    public int TrashNID { get; set; }
    public string TrashName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool Organic { get; set; }
    public string Image { get; set; } = string.Empty;
    public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
}
