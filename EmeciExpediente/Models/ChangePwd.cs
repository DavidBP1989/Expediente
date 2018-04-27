using System.ComponentModel.DataAnnotations;

public class ChangePwd
{
    [Required]
    public string oldPassword { get; set; }
    [Required]
    public string newPassword { get; set; }
    [Required]
    public string newPassword2 { get; set; }



    public bool errorFields { get; set; } = false;
    public bool errorNewPassword { get; set; } = false;
    public bool errorOldPassword { get; set; } = false;
    public bool success { get; set; } = false;

    public ChangePwd()
    {

    }
}