using System;
using System.Collections.Generic;
using static System.IO.Directory;
using System.IO;
using System.Web.Mvc;
using System.Drawing;
using System.Dynamic;
using System.Web;
using static System.Configuration.ConfigurationManager;
using EmeciExpediente.Models;
using static EmeciExpediente.Models.ImagesAppModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace EmeciExpediente.Controllers
{
    public class ImagesController : BaseController
    {


#region App

        const int MaxLength_Image = 1024 * 2048;

        public int SessionId => (int)Session["IdPatient"];

        

        [HttpGet]
        public ActionResult Generales()
        {
            ImagesAppModel Model = new ImagesAppModel()
            {
                Category = TypeGallery.generales,
                Images = GetImages(TypeGallery.generales)
            };

            if (TempData["ErrorWhenAdding"] != null)
                Model.Error = TempData["ErrorWhenAdding"].ToString();

            return View("ImageContent", Model);
        }

        [HttpPost]
        public ActionResult Generales(ImagesAppModel Model)
        {
            if (ModelState.IsValid)
                AddImage(TypeGallery.generales, Model);

            return RedirectToAction("Generales");
        }

        [HttpGet]
        public ActionResult Recetas()
        {
            ImagesAppModel Model = new ImagesAppModel()
            {
                Category = TypeGallery.recetas,
                Images = GetImages(TypeGallery.recetas)
            };

            if (TempData["ErrorWhenAdding"] != null)
                Model.Error = TempData["ErrorWhenAdding"].ToString();

            return View("ImageContent", Model);
        }

        [HttpPost]
        public ActionResult Recetas(ImagesAppModel Model)
        {
            if (ModelState.IsValid)
                AddImage(TypeGallery.recetas, Model);

            return RedirectToAction("Recetas");
        }

        [HttpGet]
        public ActionResult Laboratorio()
        {
            ImagesAppModel Model = new ImagesAppModel()
            {
                Category = TypeGallery.laboratorio,
                Images = GetImages(TypeGallery.laboratorio)
            };

            if (TempData["ErrorWhenAdding"] != null)
                Model.Error = TempData["ErrorWhenAdding"].ToString();

            return View("ImageContent", Model);
        }

        [HttpPost]
        public ActionResult Laboratorio(ImagesAppModel Model)
        {
            if (ModelState.IsValid)
                AddImage(TypeGallery.laboratorio, Model);

            return RedirectToAction("Laboratorio");
        }

        [HttpGet]
        public ActionResult Diagnosticos()
        {
            ImagesAppModel Model = new ImagesAppModel()
            {
                Category = TypeGallery.diagnosticos,
                Images = GetImages(TypeGallery.diagnosticos)
            };

            if (TempData["ErrorWhenAdding"] != null)
                Model.Error = TempData["ErrorWhenAdding"].ToString();

            return View("ImageContent", Model);
        }

        [HttpPost]
        public ActionResult Diagnosticos(ImagesAppModel Model)
        {
            if (ModelState.IsValid)
                AddImage(TypeGallery.diagnosticos, Model);

            return RedirectToAction("Diagnosticos");
        }

        [HttpGet]
        public ActionResult Medicamentos()
        {
            ImagesAppModel Model = new ImagesAppModel()
            {
                Category = TypeGallery.medicamentos,
                Images = GetImages(TypeGallery.medicamentos)
            };

            if (TempData["ErrorWhenAdding"] != null)
                Model.Error = TempData["ErrorWhenAdding"].ToString();

            return View("ImageContent", Model);
        }

        [HttpPost]
        public ActionResult Medicamentos(ImagesAppModel Model)
        {
            if (ModelState.IsValid)
                AddImage(TypeGallery.medicamentos, Model);

            return RedirectToAction("Medicamentos");
        }

        [HttpGet]
        public ActionResult Vacunas()
        {
            ImagesAppModel Model = new ImagesAppModel()
            {
                Category = TypeGallery.vacunas,
                Images = GetImages(TypeGallery.vacunas)
            };

            if (TempData["ErrorWhenAdding"] != null)
                Model.Error = TempData["ErrorWhenAdding"].ToString();

            return View("ImageContent", Model);
        }

        [HttpPost]
        public ActionResult Vacunas(ImagesAppModel Model)
        {
            if (ModelState.IsValid)
                AddImage(TypeGallery.vacunas, Model);

            return RedirectToAction("Vacunas");
        }

        [HttpPost]
        public ActionResult Delete(string Image, string Category)
        {
            bool IsSuccess = DeleteImage(Image, Category);
            return Json(new { status = IsSuccess }, JsonRequestBehavior.AllowGet);
        }


        

        List<dynamic> GetImages(TypeGallery Category)
        {
            List<dynamic> Images = new List<dynamic>();

            try
            {
                string CategoryFolder = Enum.GetName(typeof(TypeGallery), Category);

                string RouteImages = $"{AppSettings["RouteImagesApp"]}/{SessionId}/{CategoryFolder}";
                string Folder = $"{AppSettings["FolderImagesApp"]}{SessionId}\\{CategoryFolder}";

                if (Exists(Folder))
                {
                    foreach(string Image in GetFiles(Folder).OrderByDescending(x => new FileInfo(x).CreationTime))
                    {
                        if (!((System.IO.File.GetAttributes(Image) & FileAttributes.Hidden) == FileAttributes.Hidden))
                        {
                            dynamic Row = new ExpandoObject();

                            string Extension = Path.GetExtension(Image);
                            string ImageName = Path.GetFileName(Image);

                            string FolderThumbnail = $"{Folder}\\Thumbnail";
                            if (!Exists(FolderThumbnail))
                                CreateDirectory(FolderThumbnail);

                            if (!ExistImage(FolderThumbnail, ImageName))
                                CreateThumbnail(Folder, ImageName);
                            if (Extension.ToLower() != ".pdf")
                            {
                                Row.Thumbnail = $"{RouteImages}/Thumbnail/{ImageName}";
                                Row._Blank = false;
                            }
                            else
                            {
                                Row.Thumbnail = $"{AppSettings["RouteImagesApp"]}/pdfDefault.png";
                                Row._Blank = true;
                            }

                            Row.Href = $"{RouteImages}/{ImageName}";
                            Row.Name = ImageName;
                            Row.ShowName = Path.GetFileNameWithoutExtension(ImageName);
                            Images.Add(Row);
                        }
                    }
                }
            }
            catch { }

            return Images;
        }

        void AddImage(TypeGallery Category, ImagesAppModel Model)
        {
            try
            {
                if (AllowLength_Image(Model.File.ContentLength))
                {
                    string CategoryFolder = Enum.GetName(typeof(TypeGallery), Category);
                    string Folder = $"{AppSettings["FolderImagesApp"]}{SessionId}\\{CategoryFolder}";
                    if (!Exists(Folder))
                        CreateDirectory(Folder);

                    string Extension = Path.GetExtension(Model.File.FileName);
                    string Rename = $"{CheckFileName(Model.ImageName)}{Extension}";

                    if (!ExistImage(Folder, Rename))
                    {
                        Model.File.SaveAs(Path.Combine(Folder, Rename));
                        if (Extension.ToLower() != ".pdf")
                            CreateThumbnail(Folder, Rename);
                    }
                    else TempData["ErrorWhenAdding"] = "Ya existe una imagen con ese nombre";
                }
                else TempData["ErrorWhenAdding"] = "La imagen no puede ser mayor a 2MB";
            }
            catch { TempData["ErrorWhenAdding"] = "Error, no se puedo agregar la imagen"; }
        }

        bool AllowLength_Image(int ContentLength) => ContentLength > MaxLength_Image ? false : true;

        bool ExistImage(string Folder, string ImageName)
        {
            foreach(string File in GetFiles(Folder))
            {
                if (Path.GetFileName(File).ToLower() == ImageName.ToLower())
                    return true;
            }

            return false;
        }

        void CreateThumbnail(string Folder, string ImageName)
        {
            try
            {
                int Height = int.Parse(AppSettings["heightThumb"]);
                int Width = int.Parse(AppSettings["widthThumb"]);

                Image Img = Image.FromFile(Path.Combine(Folder, ImageName));
                Image Thumb = Img.GetThumbnailImage(Width, Height, null, IntPtr.Zero);
                Img.Dispose();

                Folder += "\\Thumbnail";
                if (!Exists(Folder))
                    CreateDirectory(Folder);

                Thumb.Save(Path.Combine(Folder, ImageName));
            }
            catch { }
        }

        bool DeleteImage(string Image, string Category)
        {
            bool IsSuccess = false;
            try
            {
                Image = HttpUtility.UrlDecode(Image);
                if (!string.IsNullOrEmpty(Image))
                {
                    string Folder = $"{AppSettings["FolderImagesApp"]}{SessionId}\\{Category}";
                    if (Exists(Folder))
                    {
                        string _File = Path.Combine(Folder, Image);
                        System.IO.File.Delete(_File);

                        IsSuccess = true;
                        Folder += "\\Thumbnail";
                        if (Exists(Folder))
                        {
                            _File = Path.Combine(Folder, Image);
                            System.IO.File.Delete(_File);
                        }
                    }
                }
            }
            catch { }

            return IsSuccess;
        }

        string CheckFileName(string ImageName)
        {
            Regex illegalInFileName =
                        new Regex(string.Format("[{0}]", Regex.Escape(new string(Path.GetInvalidFileNameChars()))), RegexOptions.Compiled);
            return $"{illegalInFileName.Replace(ImageName, "")}";
        }

        #endregion




        #region Paciente

        [HttpPost]
        public ActionResult ChangeImgPatient(HttpPostedFileBase FileInput)
        {
            int status;
            try
            {
                string Folder = $"{AppSettings["FolderImagesApp"]}{SessionId}\\Perfil";
                if (Exists(Folder))
                {
                    //Eliminar toda las posibles imagenes.
                    if (GetFiles(Folder).Length > 0)
                    {
                        foreach (string File in GetFiles(Folder))
                        {
                            System.IO.File.Delete(File);
                        }
                    }
                    
                } else CreateDirectory(Folder);

                if (AllowLength_Image(FileInput.ContentLength))
                {
                    var File = Path.Combine(Folder, CheckFileName(FileInput.FileName));
                    FileInput.SaveAs(File);
                    status = 200;
                }
                else status = 415;

            }
            catch { status = 500; }

            return Json(new { status = status.ToString() }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}