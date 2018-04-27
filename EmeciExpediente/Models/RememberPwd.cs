using System.ComponentModel.DataAnnotations;

public class RememberPwd
{
    [Required]
    public string numberCard1 { get; set; }
    [Required]
    public string numberCard2 { get; set; }
    [Required]
    public string numberCard3 { get; set; }



    public bool accessDenied { get; set; } = false;
    public bool expiredCard { get; set; } = false;
    public bool success { get; set; } = false;
    public bool errorMail { get; set; } = false;

    public RememberPwd()
    {

    }
}