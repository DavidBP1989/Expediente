using System.ComponentModel.DataAnnotations;

public class Access
{

    [Required]
    public string numberCard1 { get; set; }
    [Required]
    public string numberCard2 { get; set; }
    [Required]
    public string numberCard3 { get; set; }
    [Required]
    public string password { get; set; }



    public bool accessDenied { get; set; } = false;
    public bool expiredCard { get; set; } = false;


    public Access()
    {

    }
}