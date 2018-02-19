using DomainModels;
using System;
using System.IO;
using System.Web;
using System.Web.Helpers;

namespace Services.Implementation
{
    public class ImageProcessing : IImageProcessing
    {
        private readonly IProductsService _productservice;

        public ImageProcessing(IProductsService productservice)
        {
            _productservice = productservice;
        }

        public void GenerateDirectoriesAndSaveImages(HttpPostedFileBase file, int productid)
        {
            DirectoryInfo originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", AppDomain.CurrentDomain.BaseDirectory));

            string pathStringProducts = Path.Combine(originalDirectory.ToString(), "Products");
            string pathStringProductsId = Path.Combine(originalDirectory.ToString(), "Products\\" + productid.ToString());
            string pathStringThumbs = Path.Combine(originalDirectory.ToString(), "Products\\" + productid.ToString() + "\\Thumbs");
            string pathStringGallery = Path.Combine(originalDirectory.ToString(), "Products\\" + productid.ToString() + "\\Gallery");
            string pathStringGalleryThumbs = Path.Combine(originalDirectory.ToString(), "Products\\" + productid.ToString() + "\\Gallery\\Thumbs");

            if (!Directory.Exists(pathStringProducts))
                Directory.CreateDirectory(pathStringProducts);

            if (!Directory.Exists(pathStringProductsId))
                Directory.CreateDirectory(pathStringProductsId);

            if (!Directory.Exists(pathStringThumbs))
                Directory.CreateDirectory(pathStringThumbs);

            if (!Directory.Exists(pathStringGallery))
                Directory.CreateDirectory(pathStringGallery);

            if (!Directory.Exists(pathStringGalleryThumbs))
                Directory.CreateDirectory(pathStringGalleryThumbs);

            // Verify extension
            if (file.ContentType.ToLower() == "image/jpg" ||
                file.ContentType.ToLower() == "image/jpeg" ||
                file.ContentType.ToLower() == "image/gif" ||
                file.ContentType.ToLower() == "image/png")
            {

                Product justinsertedproduct = _productservice.GetProduct(productid);
                justinsertedproduct.ImageName = file.FileName;

                // Set original and thumb image paths
                var path = string.Format("{0}\\{1}", pathStringProductsId, file.FileName);
                file.SaveAs(path);
                var path2 = string.Format("{0}\\{1}", pathStringThumbs, file.FileName);

                // Create and save thumb
                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200);
                img.Save(path2);
            }
        }
    }
}
