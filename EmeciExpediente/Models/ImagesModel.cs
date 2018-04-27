using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

public class ImagesModel
{
    [Required]
    public HttpPostedFileBase file { get; set; }
    public List<dynamic> images { get; set; }
    public int typeScanning { get; set; }
    [Required]
    public string nameImage { get; set; }

    public ImagesModel()
    {

    }
}