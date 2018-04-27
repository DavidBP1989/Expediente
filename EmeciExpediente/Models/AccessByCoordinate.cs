using System.ComponentModel.DataAnnotations;

public class AccessByCoordinate
{
    [Required]
    public string coordinate { get; set; }
    public bool accessDenied { get; set; } = false;


    public AccessByCoordinate()
    {

    }
}