namespace ProductManagement.UI.Models;

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public DateTime CreatedDate { get; set; }


    public bool IsAvailable { get; set; }


}