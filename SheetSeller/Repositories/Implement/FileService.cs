using SheetSeller.Repositories.Abstract;
using SheetSeller.Models.DTO;

namespace SheetSeller.Repositories.Implement
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment environment;
        public FileService(IWebHostEnvironment env)
        {
            this.environment = env;
        }
        public Status SaveImage(IFormFile imageFile, string ID)
        {
            try
            {
                var wwwPath = this.environment.WebRootPath;
                var path = Path.Combine(wwwPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                // Check the allowed extenstions
                var ext = Path.GetExtension(imageFile.FileName);
                var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
                if (!allowedExtensions.Contains(ext))
                {
                    string msg = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtensions));
                    return new Status(){ Message = msg,StatusCode=0 };
                };
                // we are trying to create a unique filename here
                var FileName = ID + ext;
                var fileWithPath = Path.Combine(path, FileName);
                var stream = new FileStream(fileWithPath, FileMode.Create);
                imageFile.CopyTo(stream);
                stream.Close();
                return new Status(){ StatusCode=1,Message=FileName};
            }
            catch
            {
                return new Status() { StatusCode=0,Message= "Can`t save your iage" };
            }
        }

        public Status DeleteImage(string imageFileName)
        {
            try
            {
                var wwwPath = this.environment.WebRootPath;
                var path = Path.Combine(wwwPath, "Uploads\\", imageFileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    return new Status() { StatusCode = 1, Message = "Deleted succes" };
                }
                return new Status() { StatusCode = 0, Message = "File not found" };
            }
            catch 
            {
                return new Status() { StatusCode = 0, Message = " Some error occured" };
            }
        }
    }
}
