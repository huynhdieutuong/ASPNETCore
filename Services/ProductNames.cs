using System.Collections.Generic;

public class ProductNames // 3.1 Create ProductNames service
{
    private List<string> names { get; set; }

    public ProductNames()
    {
        names = new List<string>()
        {
            "Iphone X",
            "Samsung Note 10",
            "Nokia 3310"
        };
    }

    public List<string> GetNames() => names;
}