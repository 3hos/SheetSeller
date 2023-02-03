using SheetSeller.Repositories.Abstract;
using SheetSeller.Models.DTO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

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
                using var image = Image.Load(imageFile.OpenReadStream());
                if(image.Height>image.Width)
                {
                    image.Mutate(x => x.Crop(image.Width, image.Height-(image.Height - image.Width)/2));
                    image.Mutate(x => x.Flip(FlipMode.Vertical));
                    image.Mutate(x => x.Crop(image.Width, image.Width));
                    image.Mutate(x => x.Flip(FlipMode.Vertical));
                }
                else if (image.Width > image.Height)
                {
                    image.Mutate(x => x.Crop(image.Height, image.Width-(image.Width - image.Height)/2));
                    image.Mutate(x => x.Flip(FlipMode.Horizontal));
                    image.Mutate(x => x.Crop(image.Height, image.Height));
                    image.Mutate(x => x.Flip(FlipMode.Horizontal));
                }
                image.Mutate(x => x.Resize(512, 512));
                var FileName = ID + ext;
                var fileWithPath = Path.Combine(path, FileName);
                image.Save(fileWithPath);
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
