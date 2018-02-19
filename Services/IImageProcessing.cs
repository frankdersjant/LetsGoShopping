using System.Web;

namespace Services
{
    public interface IImageProcessing
    {
        void GenerateDirectoriesAndSaveImages(HttpPostedFileBase file, int productid);
    }
}
