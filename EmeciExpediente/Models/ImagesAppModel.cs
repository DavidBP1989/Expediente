using System.Collections.Generic;
using System.Web;

namespace EmeciExpediente.Models
{
    public class ImagesAppModel
    {
        public enum TypeGallery
        {
            generales,
            recetas,
            laboratorio,
            diagnosticos,
            medicamentos,
            vacunas
        }

        public TypeGallery Category { get; set; }
        public List<dynamic> Images { get; set; }
        public HttpPostedFileBase File { get; set; }
        public string ImageName { get; set; }
        public string Error { get; set; }
    }
}